import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './users.html',
  styleUrl: './users.css'
})
export class UsersComponent implements OnInit {

  private userService = inject(UserService);

  // ── Modal control ─────────────────────────────
  showModal = false;
  isEditMode = false;
  editingId = '';

  // ── Toast ─────────────────────────────────────
  toastMessage = '';
  toastType = '';

  // ── Form ──────────────────────────────────────
  form = {
    name: '',
    email: '',
    password: '',
    phone: '',
    status: 'Active',
    role: 'MEMBER'
  };

  // ── Users ─────────────────────────────────────
  users: User[] = [];

  ngOnInit() {
    this.loadUsers();
  }

  // ── Load Users ────────────────────────────────
  loadUsers() {
    this.userService.getUsers().subscribe({
      next: (data) => this.users = data,
      error: () => this.showToast('Failed to load users', 'error')
    });
  }


  selectedRole: string = 'ALL';
searchText: string = '';

get filteredUsers(): User[] {
  return this.users.filter(user => {

    const roleMatch =
      this.selectedRole === 'ALL' ||
      user.role === this.selectedRole;

    const searchMatch =
      user.fullName.toLowerCase().includes(this.searchText.toLowerCase()) ||
      user.email.toLowerCase().includes(this.searchText.toLowerCase());

    return roleMatch && searchMatch;
  });
}
  // ── Open Add Modal ────────────────────────────
  openAddModal() {
    this.isEditMode = false;
    this.editingId = '';

    this.form = {
      name: '',
      email: '',
      password: '',
      phone: '',
      status: 'Active',  
      role: 'MEMBER'
    };

    this.showModal = true;
  }

  // ── Open Edit Modal ───────────────────────────
  openEditModal(user: User) {
    this.isEditMode = true;
    this.editingId = user.id;

    this.form = {
      name: user.fullName,
      email: user.email,
      password: '',
      phone: '',
      status: user.status, 
      role: user.role
    };

    this.showModal = true;
  }

  // ── Close Modal ───────────────────────────────
  closeModal() {
    this.showModal = false;
  }

  // ── Save User ────────────────────────────────
  saveUser() {

    const name = this.form.name.trim();

    // ── NAME VALIDATION ─────────────────────────
    if (!name) {
      this.showToast('Name is required!', 'error');
      return;
    }

    if (name.length < 2) {
      this.showToast('Name must be at least 2 characters!', 'error');
      return;
    }

    if (name.length > 50) {
      this.showToast('Name too long!', 'error');
      return;
    }

    const namePattern = /^[A-Za-z]+(?: [A-Za-z]+)*$/;

    if (!namePattern.test(name)) {
      this.showToast('Name should contain only letters and single spaces!', 'error');
      return;
    }

    // ── REQUIRED VALIDATION ─────────────────────
    if (
      !this.form.name?.trim() ||
      !this.form.email?.trim() ||
      !this.form.role ||
      (!this.isEditMode && !this.form.password)
    ) {
      this.showToast('Please fill all required fields correctly!', 'error');
      return;
    }

    // ── EMAIL VALIDATION ────────────────────────
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailPattern.test(this.form.email)) {
      this.showToast('Invalid email format!', 'error');
      return;
    }

    // ── PASSWORD VALIDATION ─────────────────────
    if (!this.isEditMode && this.form.password.length < 6) {
      this.showToast('Password must be at least 6 characters!', 'error');
      return;
    }

    // ───── UPDATE ─────
    if (this.isEditMode) {

      const updatedUser = {
        fullName: this.form.name,
        email: this.form.email,
        phone: this.form.phone,
        address: '',
        externalId: '',
        role: this.form.role,
        status: this.form.status   
      };

      this.userService.updateUser(this.editingId, updatedUser).subscribe({
        next: () => {
          this.showToast('User updated successfully!', 'success');
          this.loadUsers();
          this.closeModal();
        },
        error: (err) => {
  this.showToast(this.getErrorMessage(err), 'error');
}
      });
    }

    // ───── CREATE ─────
    else {

      const newUser = {
        fullName: this.form.name,
        email: this.form.email,
        password: this.form.password,
        phone: this.form.phone,
        address: null,
        externalId: null,
        role: this.form.role,
        status: this.form.status   
      };

      this.userService.createUser(newUser).subscribe({
        next: () => {
          this.showToast('User created successfully!', 'success');
          this.loadUsers();
          this.closeModal();
        },
        error: (err) => {
  this.showToast(this.getErrorMessage(err), 'error');
}
      });
    }
  }


  getErrorMessage(err: any): string {
  if (typeof err.error === 'string') return err.error;

  if (err.error?.message) return err.error.message;

  if (err.error?.title) return err.error.title;

  if (err.error?.errors) {
    const key = Object.keys(err.error.errors)[0];
    return err.error.errors[key][0];
  }

  return 'Something went wrong';
}

  // ── Delete ───────────────────────────────────
 deleteUser(id: string) {
  this.userService.deleteUser(id).subscribe({
    next: () => {
      this.loadUsers();

      this.showToast('User deleted successfully!', 'success');
    },
    error: (err) => {
      this.showToast(this.getErrorMessage(err), 'error');
    }
  });
}
  // ── Toast ────────────────────────────────────
  showToast(message: string, type: string) {
    this.toastMessage = message;
    this.toastType = type;

    setTimeout(() => this.toastMessage = '', 2500);
  }

  // ── Avatar Initial ───────────────────────────
  getInitial(name: string): string {
    return name.charAt(0).toUpperCase();
  }

  // ── Active Count ─────────────────────────────
  get activeCount(): number {
    return this.users.filter(u => u.status === 'Active').length; 
  }
}