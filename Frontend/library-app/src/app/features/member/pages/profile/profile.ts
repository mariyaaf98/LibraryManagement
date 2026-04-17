import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../../../core/services/user.service';
import { UserResponseDto } from '../../../../core/models/user-response.dto';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class ProfileComponent implements OnInit {

  user: UserResponseDto | null = null;

  // Password fields
  currentPassword = '';
  newPassword = '';
  message: string = '';
  messageType: 'success' | 'error' | '' = '';
  currentPasswordError: string = '';

  constructor(
    private userService: UserService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadProfile();
  }

  // Load profile
  loadProfile() {
    const storedUser = localStorage.getItem('user');
    if (!storedUser) return;

    const parsedUser = JSON.parse(storedUser);

    this.userService.getCurrentUser(parsedUser.email).subscribe({
      next: (res) => {
        this.user = res;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        this.cdr.detectChanges();
      }
    });
  }

  // Change password
  changePassword() {
  const data = {
    email: this.user?.email,
    currentPassword: this.currentPassword,
    newPassword: this.newPassword
  };

  // reset old messages
  this.currentPasswordError = '';
  this.message = '';
  this.messageType = '';

  this.userService.changePassword(data).subscribe({
    next: () => {
      this.message = 'Password reset successfully';
      this.messageType = 'success';

      this.currentPassword = '';
      this.newPassword = '';
       this.cdr.detectChanges();
    },

    error: (err) => {
      console.error(err);

      const errorMsg = err.error?.message || 'Invalid current password';

      this.currentPasswordError = errorMsg;
      this.cdr.detectChanges();
    }
  });
}
}