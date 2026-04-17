import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class LoginGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(): boolean {
    const role = localStorage.getItem('role');

    if (role) {
      // already logged in → redirect
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