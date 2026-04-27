import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError, finalize } from 'rxjs';
import { AuthService } from '../services/auth.service';

let isRefreshing = false;

export const credentialsInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  // Skip external APIs
  if (req.url.includes('api.cloudinary.com')) {
    return next(req);
  }

  const clonedReq = req.clone({
    withCredentials: true
  });

  return next(clonedReq).pipe(
    catchError((error: HttpErrorResponse) => {

      if (error.status === 401 && !req.url.includes('/auth/refresh')) {

        // 🚫 Prevent multiple refresh calls
        if (isRefreshing) {
          return throwError(() => error);
        }

        isRefreshing = true;

        return authService.refreshToken().pipe(
          switchMap(() => {
            // 🔁 Retry request
            return next(clonedReq);
          }),
          catchError((refreshError) => {
            // ❌ Refresh failed → logout
            authService.logout().subscribe(); // ✅ ensure execution
            return throwError(() => refreshError);
          }),
          finalize(() => {
            isRefreshing = false;
          })
        );
      }

      return throwError(() => error);
    })
  );
};