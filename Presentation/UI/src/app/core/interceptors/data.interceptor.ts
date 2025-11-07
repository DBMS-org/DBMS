import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { map } from 'rxjs/operators';

// Unwraps the standard ApiResponse<T> so that components/services receive raw T
export const dataInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    map((event) => {
      if (event instanceof HttpResponse) {
        const body: any = event.body;
        if (body && typeof body === 'object' && 'data' in body) {
          // Replace body with the inner data object
          return event.clone({ body: body.data });
        }
      }
      return event;
    })
  );
}; 