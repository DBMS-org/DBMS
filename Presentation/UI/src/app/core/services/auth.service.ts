import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { User, LoginResponse } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    // Load user from localStorage on service initialization
    this.loadCurrentUser();
  }

  login(username: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/api/auth/login`, { username, password });
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
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
    return userRole?.toLowerCase() === role.toLowerCase();
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