// admin/librarians/librarians.ts
// Librarians page — Add, Edit/Update, Delete librarians using API (DTO based)

import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-librarians',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './librarians.html',
  styleUrl: './librarians.css'
})
export class LibrariansComponent implements OnInit {

  private userService = inject(UserService);

  // ── Modal control ─────────────────────────────
  showModal = false;
  isEditMode = false;
  editingId = '';

  // ── Toast notification ────────────────────────
  toastMessage = '';
  toastType = '';

  // ── Form fields ───────────────────────────────
  form = {
    name: '',
    email: '',
    phone: '',
    branch: '',
    status: 'ACTIVE'
  };

  // ── Librarians list ───────────────────────────
  librarians: User[] = [];

  // ── Branch options ────────────────────────────
  branches = [
    'Main Branch',
    'East Wing',
    'West Wing',
    'North Block',
    'South Block'
  ];

  // ── Load librarians ───────────────────────────
  ngOnInit() {
    this.loadLibrarians();
  }

  loadLibrarians() {
    this.userService.getUsers().subscribe({
      next: (data) => {
        // only librarians (DTO has no isDeleted)
        this.librarians = data.filter(
          user => user.role === 'LIBRARIAN'
        );
      },
      error: () => {
        this.showToast('Failed to load librarians', 'error');
      }
    });
  }

  // ── Open Add Modal ────────────────────────────
  openAddModal() {
    this.isEditMode = false;
    this.editingId = '';

    this.form = {
      name: '',
      email: '',
      phone: '',
      branch: '',
      status: 'ACTIVE'
    };

    this.showModal = true;
  }

  // ── Open Edit Modal ───────────────────────────
  openEditModal(lib: User) {
    this.isEditMode = true;
    this.editingId = lib.id;

    this.form = {
      name: lib.fullName,
      email: lib.email,
      phone: '', // not available in DTO
      branch: '',
      status: lib.status
    };

    this.showModal = true;
  }

  // ── Close Modal ───────────────────────────────
  closeModal() {
    this.showModal = false;
  }

  // ── Save Librarian (Create / Update) ──────────
  saveLibrarian() {

    if (!this.form.name || !this.form.email) {
      this.showToast('Please fill Name and Email!', 'error');
      return;
    }

    // ───── UPDATE ─────
    if (this.isEditMode) {

      const updatedUser = {
        fullName: this.form.name,
        email: this.form.email,
        phone: this.form.phone,
        address: this.form.branch,
        externalId: ''
      };

      this.userService.updateUser(this.editingId, updatedUser).subscribe({
        next: () => {
          this.showToast('Librarian updated successfully!', 'success');
          this.loadLibrarians();
          this.closeModal();
        },
        error: () => {
          this.showToast('Update failed!', 'error');
        }
      });
    }

    // ───── CREATE ─────
    else {

      const newUser = {
        fullName: this.form.name,
        email: this.form.email,
        password: '123456', // required by backend
        phone: this.form.phone,
        address: this.form.branch,
        externalId: ''
      };

      this.userService.createUser(newUser).subscribe({
        next: () => {
          this.showToast('Librarian created successfully!', 'success');
          this.loadLibrarians();
          this.closeModal();
        },
        error: () => {
          this.showToast('Create failed!', 'error');
        }
      });
    }
  }

  // ── Delete Librarian ──────────────────────────
  deleteLibrarian(id: string) {
    this.userService.deleteUser(id).subscribe({
      next: () => {
        this.showToast('Librarian deleted.', 'success');
        this.loadLibrarians();
      },
      error: () => {
        this.showToast('Delete failed!', 'error');
      }
    });
  }

  // ── Toast helper ─────────────────────────────
  showToast(message: string, type: string) {
    this.toastMessage = message;
    this.toastType = type;

    setTimeout(() => {
      this.toastMessage = '';
    }, 2500);
  }

  // ── Avatar initial ───────────────────────────
  getInitial(name: string): string {
    return name.charAt(0).toUpperCase();
  }

  // ── Active count ─────────────────────────────
  get activeCount(): number {
    return this.librarians.filter(l => l.status === 'ACTIVE').length;
  }

}