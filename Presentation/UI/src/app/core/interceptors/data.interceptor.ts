import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { map } from 'rxjs/operators';

// Unwraps ApiResponse<T> to return raw data
export const dataInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    map((event) => {
      if (event instanceof HttpResponse) {
        const body: any = event.body;
        if (body && typeof body === 'object' && 'data' in body) {
          return event.clone({ body: body.data });
        }
      }
      return event;
    })
  );
}; 