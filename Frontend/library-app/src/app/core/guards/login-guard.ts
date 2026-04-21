
import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const LoginGuard: CanActivateFn = () => {

  const router = inject(Router);
  const role = localStorage.getItem('role');

  if (role) {
    router.navigate([role === 'ADMIN' ? '/admin' : '/member'], {
      replaceUrl: true
    });
    return false;
  }

  return true;
};