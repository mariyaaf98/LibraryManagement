import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { UserResponseDto } from '../../../../core/models/user-response.dto';
import { UserService } from '../../../../core/services/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'user-list',
  standalone: true,
  imports: [CommonModule], 
  templateUrl: './user-list.html',
  styleUrls: ['./user-list.css']
})
export class UserListComponent implements OnInit {

  users: UserResponseDto[] = [];
  loading = false;
  errorMessage = '';

  constructor(
    private userService: UserService,
    private cdr: ChangeDetectorRef   
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this.errorMessage = '';

    this.userService.getUsers().subscribe({
      next: (res: UserResponseDto[]) => {
        this.users = res;
        this.loading = false;

        this.cdr.detectChanges();   
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Failed to load users';
        this.loading = false;

        this.cdr.detectChanges();   
      }
    });
  }

  toggleBlock(user: UserResponseDto): void {

    this.userService.toggleBlock(user.id).subscribe({
      next: () => {

        user.status =
          user.status === 'Blocked'
            ? 'Active'
            : 'Blocked';

        this.cdr.detectChanges();   
      },
      error: (err) => {
        console.error(err);
        alert('Failed to update status');
      }
    });
  }

}