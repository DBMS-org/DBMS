import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { User, LoginResponse } from '../models/user.model';
import { SessionService } from './session.service';
import { AppDialogService } from './dialog.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router,
    private dialogService: AppDialogService,
    private sessionService: SessionService
  ) {
    // Load user from localStorage on service initialization
    this.loadCurrentUser();
  }

  login(username: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/api/auth/login`, { username, password });
  }

  logout(): void {
    const token = this.getToken();
    
    // Call backend logout endpoint if token exists
    if (token) {
      this.http.post(`${environment.apiUrl}/api/auth/logout`, {}).subscribe({
        next: (response) => {
          console.log('Server logout successful:', response);
        },
        error: (error) => {
          console.warn('Server logout failed, but continuing with client logout:', error);
        },
        complete: () => {
          this.performClientLogout();
        }
      });
    } else {
      this.performClientLogout();
    }
  }

  logoutWithConfirmation(): void {
    const currentUser = this.getCurrentUser();

    this.dialogService.openLogoutDialog(currentUser?.name).subscribe(confirmed => {
      if (confirmed) {
        this.logout();
      }
    });
  }

  private performClientLogout(): void {
    // Backup session data for debugging (optional)
    const sessionBackup = this.sessionService.backupSessionData();
    console.log('Session backup created:', sessionBackup);

    // Clear all session data comprehensively
    this.sessionService.clearAllSessionData();
    
    // Reset user subject
    this.currentUserSubject.next(null);
    
    // Navigate to login
    this.router.navigate(['/login']);
    
    console.log('Logout completed successfully');
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return !!token; // Basic check - full validation happens server-side
  }

  // New method for server-side token validation
  validateToken(): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/api/auth/validate-token`, {});
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  setCurrentUser(user: User, token: string): void {
    localStorage.setItem('token', token);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  private loadCurrentUser(): void {
    const userStr = localStorage.getItem('user');
    if (userStr && this.isAuthenticated()) {
      try {
        const user = JSON.parse(userStr);
        this.currentUserSubject.next(user);
      } catch {
        this.logout();
      }
    }
  }

  getUserRole(): string | null {
    const user = this.getCurrentUser();
    return user?.role || null;
  }

  getUserRegion(): string | null {
    const user = this.getCurrentUser();
    return user?.region || null;
  }

  getUserCountry(): string | null {
    const user = this.getCurrentUser();
    return user?.country || null;
  }

  hasRole(role: string): boolean {
    const userRole = this.getUserRole();
    if (!userRole) return false;
    
    // Normalize both roles by removing spaces and converting to lowercase
    const normalizedUserRole = userRole.toLowerCase().replace(/\s+/g, '');
    const normalizedExpectedRole = role.toLowerCase().replace(/\s+/g, '');
    
    return normalizedUserRole === normalizedExpectedRole;
  }

  isAdmin(): boolean {
    return this.hasRole('admin');
  }

  isBlastingEngineer(): boolean {
    return this.hasRole('blastingengineer');
  }

  isMechanicalEngineer(): boolean {
    return this.hasRole('mechanicalengineer');
  }

  isMachineManager(): boolean {
    return this.hasRole('machinemanager');
  }

  isOperator(): boolean {
    return this.hasRole('operator');
  }

  isExplosiveManager(): boolean {
    return this.hasRole('explosivemanager');
  }

  isStoreManager(): boolean {
    return this.hasRole('storemanager');
  }

  // Forgot Password Methods
  forgotPassword(email: string): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/api/auth/forgot-password`, { email });
  }

  verifyResetCode(email: string, code: string): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/api/auth/verify-reset-code`, { email, code });
  }

  resetPassword(email: string, code: string, newPassword: string, confirmPassword: string): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/api/auth/reset-password`, { 
      email, 
      code, 
      newPassword, 
      confirmPassword 
    });
  }
}