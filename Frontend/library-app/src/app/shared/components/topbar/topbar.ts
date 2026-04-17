import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './topbar.html',
  styleUrl: './topbar.css',
})
export class TopbarComponent implements OnInit {

  userName: string = '';
  isLoggedIn: boolean = false; 

  constructor(
    private router: Router,
    private cdr: ChangeDetectorRef 
  ) {}

  ngOnInit() {
    const userData = localStorage.getItem('user');
    const role = localStorage.getItem('role');

    this.isLoggedIn = !!role; 

    if (userData && userData !== 'undefined') {
      try {
        const user = JSON.parse(userData);
        this.userName = user?.fullName || user?.name || user?.email || 'User';
      } catch (error) {
        console.error('Invalid JSON in localStorage:', error);
        this.userName = 'User';
      }
    } else {
      this.userName = 'Guest'; 
    }
  }

  logout() {
    localStorage.clear();
    this.isLoggedIn = false; 
    this.router.navigate(['/login']);
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }
}