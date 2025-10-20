import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();
  
  console.log('Auth Interceptor - Token:', token ? 'Present' : 'Missing');
  console.log('Auth Interceptor - Request URL:', req.url);
  
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    console.log('Auth Interceptor - Authorization header added');
  } else {
    console.log('Auth Interceptor - No token, skipping authorization header');
  }
  
  return next(req);
}; 