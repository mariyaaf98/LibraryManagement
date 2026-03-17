// admin/authors/authors.ts

import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthorService } from '../../services/author.service';
import { Author } from '../../models/author.model';

@Component({
  selector: 'app-authors',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './authors.html',
  styleUrl: './authors.css'
})
export class AuthorsComponent implements OnInit {

  private authorService = inject(AuthorService);

  // ── Modal ─────────────────────────────────────
  showModal  = false;
  isEditMode = false;
  editingId  = '';

  // ── Toast ─────────────────────────────────────
  toastMessage = '';
  toastType    = '';

  // ── Search ────────────────────────────────────
  searchText = '';

  // ── Form fields ───────────────────────────────
  form = {
    givenName:  '',
    familyName: '',
    fullName:   '',
    birthDate:  '',
    biography:  ''
  };

  // ── Authors list ──────────────────────────────
  authors: Author[] = [];

  // ── Load on page open ─────────────────────────
  ngOnInit() {
    this.loadAuthors();
  }

  loadAuthors() {
    this.authorService.getAuthors().subscribe(data => {
      this.authors = data;
    });
  }

  // ── Search filter — runs in HTML ──────────────
  get filteredAuthors(): Author[] {
    if (!this.searchText.trim()) return this.authors;

    const keyword = this.searchText.toLowerCase();
    return this.authors.filter(a =>
      a.fullName.toLowerCase().includes(keyword) ||
      a.givenName?.toLowerCase().includes(keyword) ||
      a.familyName?.toLowerCase().includes(keyword)
    );
  }

  // ── Open Add modal ────────────────────────────
  openAddModal() {
    this.isEditMode = false;
    this.editingId  = '';
    this.form = { givenName: '', familyName: '', fullName: '', birthDate: '', biography: '' };
    this.showModal  = true;
  }

  // ── Open Edit modal ───────────────────────────
  openEditModal(author: Author) {
    this.isEditMode = true;
    this.editingId  = author.id!;
    this.form = {
      givenName:  author.givenName  ?? '',
      familyName: author.familyName ?? '',
      fullName:   author.fullName,
      birthDate:  author.birthDate  ?? '',
      biography:  author.biography  ?? ''
    };
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  // ── Save (Add or Update) ──────────────────────
  saveAuthor() {
    if (!this.form.fullName) {
      this.showToast('Please fill in Full Name!', 'error');
      return;
    }

    if (this.isEditMode) {

      const updated: Author = {
        id:         this.editingId,
        givenName:  this.form.givenName,
        familyName: this.form.familyName,
        fullName:   this.form.fullName,
        birthDate:  this.form.birthDate  || undefined,
        biography:  this.form.biography  || undefined,
        updatedAt:  new Date().toISOString(),
        isDeleted:  false
      };

      this.authorService.updateAuthor(updated).subscribe(() => {
        this.showToast('Author updated successfully!', 'success');
        this.loadAuthors();
        this.closeModal();
      });

    } else {

      const newAuthor = {
        givenName:  this.form.givenName,
        familyName: this.form.familyName,
        fullName:   this.form.fullName,
        birthDate:  this.form.birthDate  || undefined,
        biography:  this.form.biography  || undefined,
        createdAt:  new Date().toISOString(),
        updatedAt:  new Date().toISOString(),
        isDeleted:  false
      };

      this.authorService.createAuthor(newAuthor).subscribe(() => {
        this.showToast('Author created successfully!', 'success');
        this.loadAuthors();
        this.closeModal();
      });
    }
  }

  // ── Delete ────────────────────────────────────
  deleteAuthor(id: string) {
    const index = this.authors.findIndex(a => a.id === id);
    if (index !== -1) this.authors.splice(index, 1);
    this.showToast('Author deleted.', 'error');
    this.authorService.deleteAuthor(id).subscribe();
  }

  // ── Toast ─────────────────────────────────────
  showToast(message: string, type: string) {
    this.toastMessage = message;
    this.toastType    = type;
    setTimeout(() => { this.toastMessage = ''; }, 2500);
  }

  // ── Avatar ────────────────────────────────────
  getInitial(name: string): string {
    return name ? name.charAt(0).toUpperCase() : '?';
  }
}