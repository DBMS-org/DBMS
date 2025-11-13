import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { User, CreateUserRequest, UpdateUserRequest, DatabaseConnectionTest } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly apiUrl = `${environment.apiUrl}/api/users`;

  constructor(private http: HttpClient) {}

  // Get all users
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl).pipe(
      map(users => {
        // Handle null or undefined response from backend
        if (!users || !Array.isArray(users)) {
          console.warn('Received null or invalid users data from API, returning empty array');
          return [];
        }
        return users.map(user => ({
          ...user,
          createdAt: new Date(user.createdAt),
          updatedAt: new Date(user.updatedAt)
        }));
      }),
      catchError(this.handleError)
    );
  }

  // Get user by ID
  getUser(id: number): Observable<User> {
    const headers = {
      'Cache-Control': 'no-cache, no-store, must-revalidate',
      'Pragma': 'no-cache',
      'Expires': '0'
    };
    
    // Add timestamp to URL for aggressive cache busting
    const timestamp = new Date().getTime();
    const url = `${this.apiUrl}/${id}?_t=${timestamp}`;
    
    return this.http.get<User>(url, { headers }).pipe(
      map(user => ({
        ...user,
        // Parse as UTC timestamps - the backend should be sending ISO strings with Z suffix
        createdAt: new Date(user.createdAt),
        updatedAt: new Date(user.updatedAt),
        // If lastLoginAt exists, parse it as UTC too
        lastLoginAt: user.lastLoginAt ? new Date(user.lastLoginAt) : undefined
      })),
      catchError(this.handleError)
    );
  }

  // Create new user
  createUser(userRequest: CreateUserRequest): Observable<User> {
    console.log('Sending user creation request:', userRequest);
    return this.http.post<User>(this.apiUrl, userRequest).pipe(
      map(user => ({
        ...user,
        createdAt: new Date(user.createdAt),
        updatedAt: new Date(user.updatedAt)
      })),
      catchError(this.handleError)
    );
  }

  // Update existing user
  updateUser(id: number, userRequest: UpdateUserRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, userRequest).pipe(
      catchError(this.handleError)
    );
  }

  // Delete user
  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  // Test database connection
  testConnection(): Observable<DatabaseConnectionTest> {
    return this.http.get<DatabaseConnectionTest>(`${this.apiUrl}/test-connection`).pipe(
      map(result => ({
        ...result,
        timestamp: new Date(result.timestamp)
      })),
      catchError(this.handleError)
    );
  }

  // Error handling
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred!';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Client Error: ${error.error.message}`;
    } else {
      // Server-side error
      console.error('Full error response:', error);
      
      if (error.status === 404) {
        errorMessage = 'User not found';
      } else if (error.status === 400) {
        // Try to get specific validation errors from the response
        if (typeof error.error === 'string') {
          errorMessage = `Validation Error: ${error.error}`;
        } else if (error.error && error.error.errors) {
          // Handle model validation errors
          const validationErrors = Object.values(error.error.errors).flat();
          errorMessage = `Validation Errors: ${validationErrors.join(', ')}`;
        } else if (error.error && error.error.message) {
          errorMessage = `Validation Error: ${error.error.message}`;
        } else {
          errorMessage = 'Invalid request data - please check all required fields';
        }
      } else if (error.status === 500) {
        errorMessage = 'Server error occurred';
      } else {
        errorMessage = `Server Error Code: ${error.status}\nMessage: ${error.message}`;
      }
    }
    
    console.error('UserService Error:', errorMessage);
    return throwError(() => error);
  }
} 