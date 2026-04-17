import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class LoginComponent {

  private authService = inject(AuthService);
  private router = inject(Router);

  email: string = '';
  password: string = '';
  rememberMe: boolean = false;
  showPassword = false;

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  login() {
    const payload = {
      email: this.email,
      password: this.password
    };

    console.log('Payload:', payload);

    this.authService.login(payload).subscribe({
      next: (res: any) => {
        console.log('Full response:', res);

        const role = res.role?.toUpperCase();
        localStorage.setItem('role', role);

        const user = {
          email: this.email,
          name: this.email.split('@')[0]
        };

        localStorage.setItem('user', JSON.stringify(user));

        if (role === 'ADMIN') {
          this.router.navigate(['/admin'], { replaceUrl: true });
        } else {
          this.router.navigate(['/member'], { replaceUrl: true });
        }
      },
      error: (err) => {
        console.log("ERROR", err);

        // Extract backend message
        const message = err?.error?.message || "Login failed";

        alert(message);
      }
    });
  }


}