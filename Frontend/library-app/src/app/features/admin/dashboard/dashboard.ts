import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class AdminDashboardComponent {


  stats = [
    { title: 'Total Books', value: 120, icon: '📚' },
    { title: 'Users', value: 45, icon: '👤' },
    { title: 'Borrowed', value: 18, icon: '📦' },
    { title: 'Authors', value: 30, icon: '✍️' }
  ];

  
  activities = [
    { text: 'New book added', time: '2 hours ago' },
    { text: 'User registered', time: '5 hours ago' },
    { text: 'Book borrowed', time: '1 day ago' },
    { text: 'Author updated', time: '2 days ago' }
  ];

}