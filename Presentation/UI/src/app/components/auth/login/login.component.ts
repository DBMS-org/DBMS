import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { LoginRequest } from '../../../core/models/user.model';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { PasswordModule } from 'primeng/password';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    InputTextModule,
    ButtonModule,
    PasswordModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';

      const loginData: LoginRequest = {
        username: this.loginForm.get('username')?.value,
        password: this.loginForm.get('password')?.value
      };

      this.authService.login(loginData.username, loginData.password)
        .subscribe({
          next: (response: any) => {
            // In case the ApiResponse wrapper wasn't unwrapped for some reason
            const auth = (response && response.user && response.token) ? response : response?.data;

            if (!auth || !auth.user) {
              this.errorMessage = 'Unexpected server response. Please try again later.';
              this.isLoading = false;
              return;
            }

            console.log('Login successful:', auth);
            
            // Set current user in auth service
            this.authService.setCurrentUser(auth.user, auth.token);
            
            // Navigate based on user role (safeguard against null)
            const role = auth.user?.role;
            if (role) {
              this.navigateByRole(role);
            } else {
              this.errorMessage = 'Your account does not have an assigned role.';
            }
          },
          error: (error) => {
            console.error('Login error:', error);
            this.errorMessage = error.error?.message || 'Login failed. Please try again.';
            this.isLoading = false;
          },
          complete: () => {
            this.isLoading = false;
          }
        });
    }
  }

  private navigateByRole(role: string): void {
    console.log('User role from API:', role);
    
    switch (role.toLowerCase().replace(/\s+/g, '')) {
      case 'admin':
        this.router.navigate(['/admin/dashboard']);
        break;
      case 'blastingengineer':
        this.router.navigate(['/blasting-engineer/dashboard']);
        break;
      case 'mechanicalengineer':
        this.router.navigate(['/mechanical-engineer/dashboard']);
        break;
      case 'machinemanager':
        this.router.navigate(['/machine-manager/dashboard']);
        break;
      case 'operator':
        this.router.navigate(['/operator/dashboard']);
        break;
      case 'explosivemanager':
        this.router.navigate(['/explosive-manager/dashboard']);
        break;
      case 'storemanager':
        this.router.navigate(['/store-manager/dashboard']);
        break;
      
      default:
        this.errorMessage = `Invalid user role: ${role}. Please contact your administrator.`;
        this.authService.logout();
        this.isLoading = false;
        break;
    }
  }
}
