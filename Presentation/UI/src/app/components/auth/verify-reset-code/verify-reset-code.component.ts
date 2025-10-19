import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-verify-reset-code',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    InputTextModule,
    ButtonModule,
    MessageModule
  ],
  templateUrl: './verify-reset-code.component.html',
  styleUrls: ['./verify-reset-code.component.scss']
})
export class VerifyResetCodeComponent implements OnInit {
  verifyCodeForm: FormGroup;
  isLoading = false;
  message = '';
  messageType: 'success' | 'error' = 'success';
  email = '';
  timeLeft = 600; // 10 minutes in seconds
  timerInterval: any;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.verifyCodeForm = this.fb.group({
      code: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]]
    });
  }

  ngOnInit() {
    // Get email from query parameters
    this.route.queryParams.subscribe(params => {
      this.email = params['email'] || '';
      if (!this.email) {
        this.router.navigate(['/forgot-password']);
      }
    });

    // Start countdown timer
    this.startTimer();
  }

  ngOnDestroy() {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
    }
  }

  startTimer() {
    this.timerInterval = setInterval(() => {
      this.timeLeft--;
      if (this.timeLeft <= 0) {
        clearInterval(this.timerInterval);
      }
    }, 1000);
  }

  getFormattedTime(): string {
    const minutes = Math.floor(this.timeLeft / 60);
    const seconds = this.timeLeft % 60;
    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
  }

  onSubmit() {
    if (this.verifyCodeForm.valid && this.email) {
      this.isLoading = true;
      this.message = '';

      const code = this.verifyCodeForm.get('code')?.value;

      this.authService.verifyResetCode(this.email, code).subscribe({
        next: (response) => {
          this.message = response.message;
          this.messageType = 'success';
          
          // Navigate to reset password page after 1 second
          setTimeout(() => {
            this.router.navigate(['/reset-password'], { 
              queryParams: { email: this.email, code: code } 
            });
          }, 1000);
        },
        error: (error) => {
          this.message = error.error?.message || 'Invalid or expired code. Please try again.';
          this.messageType = 'error';
          this.isLoading = false;
        },
        complete: () => {
          this.isLoading = false;
        }
      });
    }
  }

  resendCode() {
    if (this.email) {
      this.isLoading = true;
      this.message = '';

      this.authService.forgotPassword(this.email).subscribe({
        next: (response) => {
          this.message = 'New code sent to your email address.';
          this.messageType = 'success';
          this.timeLeft = 600; // Reset timer
          this.startTimer();
        },
        error: (error) => {
          this.message = error.error?.message || 'Failed to resend code. Please try again.';
          this.messageType = 'error';
        },
        complete: () => {
          this.isLoading = false;
        }
      });
    }
  }

  goBackToForgotPassword() {
    this.router.navigate(['/forgot-password']);
  }

  // Auto-format code input
  onCodeInput(event: any) {
    let value = event.target.value.replace(/\D/g, ''); // Remove non-digits
    if (value.length > 6) {
      value = value.substring(0, 6);
    }
    this.verifyCodeForm.patchValue({ code: value });
  }
} 