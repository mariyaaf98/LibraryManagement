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
      if (!this.email || !this.password) {
        alert('Email and Password are required');
        return;
      }

      const payload = {
        email: this.email,
        password: this.password
      };
 
      this.authService.login(payload).subscribe({
        next: (res: any) => {
          console.log('Full response:', res);

          // Store role safely
          const role = res.role?.toUpperCase() || 'MEMBER';
          localStorage.setItem('role', role);

          // Store user info
          const user = {
            email: this.email,
            name: this.email.split('@')[0]
          };
          localStorage.setItem('user', JSON.stringify(user));

        
          this.router.navigate(
            [role === 'ADMIN' ? '/admin' : '/member'],
            { replaceUrl: true }
          );
        },

        error: (err) => {
          console.log("ERROR", err);

          const message =
            err?.error?.message ||
            err?.error?.title ||
            "Login failed";

          alert(message);
        }
      });
    }

  }
