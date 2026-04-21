
import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const AuthGuard: CanActivateFn = (route, state) => {

  const router = inject(Router);
  const role = localStorage.getItem('role');

  // Not logged in
  if (!role) {
    router.navigate(['/login'], { replaceUrl: true });
    return false;
  }

  // MEMBER trying ADMIN route
  if (state.url.startsWith('/admin') && role !== 'ADMIN') {
    router.navigate(['/member'], { replaceUrl: true });
    return false;
  }

  return true;
};