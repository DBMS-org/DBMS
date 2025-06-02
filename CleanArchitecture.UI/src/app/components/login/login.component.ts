import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { LoginRequest, LoginResponse } from '../../core/models/user.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
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

      this.http.post<LoginResponse>(`${environment.apiUrl}/api/auth/login`, loginData)
        .subscribe({
          next: (response: LoginResponse) => {
            console.log('Login successful:', response);
            
            // Store the token and user data in localStorage
            localStorage.setItem('token', response.token);
            localStorage.setItem('user', JSON.stringify(response.user));
            
            // Navigate based on user role
            this.navigateByRole(response.user.role);
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
      
      default:
        this.errorMessage = `Invalid user role: ${role}. Please contact your administrator.`;
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        this.isLoading = false;
        break;
    }
  }
}
