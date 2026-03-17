// app.ts
// Root component — holds sidebar + header + router-outlet

import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,      
    RouterLink,         
    RouterLinkActive    
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent { 

  // Sidebar menu items
  navItems = [
    { label: 'Dashboard',  icon: '🏠', link: '/admin/dashboard'  },
    { label: 'Users',      icon: '👤', link: '/admin/users'       },
    { label: 'Authors',    icon: '✍️', link: '/admin/authors'     },
    { label: 'Books',      icon: '📖', link: '/admin/books'       },
  ];
}