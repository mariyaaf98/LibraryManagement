import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../../../core/services/user.service';
import { UpdateUserDto } from '../../../../../core/models/user-update.dto';
import { CreateUserDto } from '../../../../../core/models/user-create.dto';

@Component({
  selector: 'app-add-user',
  imports: [],
  templateUrl: './add-user.html',
  styleUrl: './add-user.css',
})
export class UserFormComponent implements OnInit {

  mode: 'add' | 'edit' = 'add';
  userId: string | null = null;

  user = {
    fullName: '',
    email: '',
    password: '',
    role: 'MEMBER',
    phone: '',
    address: ''
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.mode = 'edit';
      this.userId = id;
      this.loadUser(id);
    }
  }

  // ✅ LOAD USER (EDIT)
  loadUser(id: string): void {
    this.userService.getUserById(id).subscribe({
      next: (res) => {
        this.user = {
          fullName: res.fullName,
          email: res.email,
          password: '',
          role: res.role,
          phone: res.phone || '',
          address: res.address || ''
        };
      },
      error: (err) => {
        console.error(err);
        alert('Failed to load user');
      }
    });
  }

  // ✅ SUBMIT (ADD + EDIT)
  submit(form: any): void {

    if (form.invalid) {
      alert('Please fill required fields');
      return;
    }

    // ✏️ EDIT
    if (this.mode === 'edit' && this.userId) {

      const payload: UpdateUserDto = {
        fullName: this.user.fullName,
        email: this.user.email,
        role: this.user.role,
        phone: this.user.phone,
        address: this.user.address
      };

      this.userService.updateUser(this.userId, payload).subscribe({
        next: () => {
          alert('User updated successfully');
          this.router.navigate(['/admin/users']);
        },
        error: () => alert('Update failed')
      });

    }
    // ➕ ADD
    else {

      const payload: CreateUserDto = {
        fullName: this.user.fullName,
        email: this.user.email,
        password: this.user.password,
        role: this.user.role,
        phone: this.user.phone,
        address: this.user.address
      };

      this.userService.createUser(payload).subscribe({
        next: () => {
          alert('User created successfully');
          this.router.navigate(['/admin/users']);
        },
        error: () => alert('Creation failed')
      });
    }
  }
}