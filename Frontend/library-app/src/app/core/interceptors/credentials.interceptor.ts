import { HttpInterceptorFn } from '@angular/common/http';

export const credentialsInterceptor: HttpInterceptorFn = (req, next) => {

  // Skip Cloudinary requests
  if (req.url.includes('api.cloudinary.com')) {
    return next(req);
  }

  const clonedReq = req.clone({
    withCredentials: true
  });

  return next(clonedReq);
};