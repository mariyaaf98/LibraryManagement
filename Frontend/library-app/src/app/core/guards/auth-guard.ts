import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(
  route: ActivatedRouteSnapshot,//Details about the current route
  state: RouterStateSnapshot //Full URL
): boolean {

  const role = localStorage.getItem('role');

  // Not logged in
  if (!role || role === 'undefined') {
    this.router.navigate(['/login'], { replaceUrl: true });
    return false;
  }

  // MEMBER trying ADMIN
  if (state.url.startsWith('/admin') && role !== 'ADMIN') {
    this.router.navigate(['/member'], { replaceUrl: true });
    return false;
  }

  //  ADMIN trying LOGIN (back button case)
  if (state.url.startsWith('/login')) {
    if (role === 'ADMIN') {
      this.router.navigate(['/admin'], { replaceUrl: true });
    } else {
      this.router.navigate(['/member'], { replaceUrl: true });
    }
    return false;
  }

  return true;
}
}