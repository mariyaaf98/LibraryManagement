// // admin/books/books.ts

// import { Component, OnInit, inject } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { FormsModule } from '@angular/forms';
// import { BookService } from '../../services/book.service';
// import { Book } from '../../models/book.model';
// import { SearchBarComponent } from '../shared/components/search-bar/search-bar';
// import { DataTableComponent } from '../shared/components/data-table/data-table';
// import { ModalComponent } from '../shared/components/modal/modal';

// @Component({
//   selector: 'app-books',
//   standalone: true,
//   imports: [CommonModule, FormsModule,SearchBarComponent,DataTableComponent,ModalComponent],
//   templateUrl: './books.html',
//   styleUrl: './books.css'
// })
// export class BooksComponent implements OnInit {

//   private bookService = inject(BookService);


//   genres = ['Fiction', 'Science', 'Technology', 'History'];

//   availability = ['Available', 'Borrowed'];

//   tableColumns = [
//   'title',
//   'isbn',
//   'publishedYear',
//   'language'
// ];

//   // ── Modal ─────────────────────────────────────
//   showModal     = false;
//   showDetailModal = false;    // for view detail popup
//   isEditMode    = false;
//   editingId     = '';
//   selectedBook: Book | null = null;   // for detail view

//   // ── Toast ─────────────────────────────────────
//   toastMessage = '';
//   toastType    = '';

//   // ── Search & Filter ───────────────────────────
//   searchText     = '';
//   filterLanguage = '';      // filter by language
//   filterYear     = '';      // filter by year

//   // ── Form ──────────────────────────────────────
//   form = {
//     title:         '',
//     subtitle:      '',
//     isbn:          '',
//     edition:       '',
//     publishedYear: '',
//     language:      '',
//     summary:       '',
//     coverImageUrl: '',
//     subCategoryId: ''
//   };

//   // ── Books list ────────────────────────────────
//   books: Book[] = [];

//   ngOnInit() {
//     this.loadBooks();
//   }

//   loadBooks() {
//     this.bookService.getBooks().subscribe(data => {
//       this.books = data;
//     });
//   }

//   // ── Search + Filter combined ──────────────────
//   get filteredBooks(): Book[] {
//     return this.books.filter(b => {

//       // search by title, isbn, author
//       const keyword = this.searchText.toLowerCase();
//       const matchSearch = !this.searchText ||
//         b.title.toLowerCase().includes(keyword) ||
//         b.isbn?.toLowerCase().includes(keyword) ||
//         b.authors?.some(a => a.toLowerCase().includes(keyword));

//       // filter by language
//       const matchLanguage = !this.filterLanguage ||
//         b.language?.toLowerCase() === this.filterLanguage.toLowerCase();

//       // filter by published year
//       const matchYear = !this.filterYear ||
//         b.publishedYear?.toString() === this.filterYear;

//       return matchSearch && matchLanguage && matchYear;
//     });
//   }

//   // ── Unique languages for filter dropdown ──────
//   get languages(): string[] {
//     const all = this.books
//       .map(b => b.language)
//       .filter(l => l) as string[];
//     return [...new Set(all)];   // remove duplicates
//   }

//   // ── Clear all filters ─────────────────────────
//   clearFilters() {
//     this.searchText     = '';
//     this.filterLanguage = '';
//     this.filterYear     = '';
//   }

//   // ── View book detail ──────────────────────────
//   viewDetail(book: Book) {
//     this.selectedBook    = book;
//     this.showDetailModal = true;
//   }

//   closeDetailModal() {
//     this.showDetailModal = false;
//     this.selectedBook    = null;
//   }

//   // ── Open Add modal ────────────────────────────
//   openAddModal() {
//     this.isEditMode = false;
//     this.editingId  = '';
//     this.form = {
//       title: '', subtitle: '', isbn: '', edition: '',
//       publishedYear: '', language: '', summary: '',
//       coverImageUrl: '', subCategoryId: ''
//     };
//     this.showModal = true;
//   }

//   // ── Open Edit modal ───────────────────────────
//   openEditModal(book: Book) {
//     this.isEditMode = true;
//     this.editingId  = book.id!;
//     this.form = {
//       title:         book.title,
//       subtitle:      book.subtitle      ?? '',
//       isbn:          book.isbn          ?? '',
//       edition:       book.edition       ?? '',
//       publishedYear: book.publishedYear?.toString() ?? '',
//       language:      book.language      ?? '',
//       summary:       book.summary       ?? '',
//       coverImageUrl: book.coverImageUrl ?? '',
//       subCategoryId: book.subCategoryId ?? ''
//     };
//     this.showModal = true;
//   }

//   closeModal() {
//     this.showModal = false;
//   }

//   // ── Save ──────────────────────────────────────
//   saveBook() {
//     if (!this.form.title) {
//       this.showToast('Please fill in Title!', 'error');
//       return;
//     }

//     const bookData = {
//       title:         this.form.title,
//       subtitle:      this.form.subtitle      || undefined,
//       isbn:          this.form.isbn          || undefined,
//       edition:       this.form.edition       || undefined,
//       publishedYear: this.form.publishedYear ? +this.form.publishedYear : undefined,
//       language:      this.form.language      || undefined,
//       summary:       this.form.summary       || undefined,
//       coverImageUrl: this.form.coverImageUrl || undefined,
//       subCategoryId: this.form.subCategoryId,
//       createdAt:     new Date().toISOString(),
//       updatedAt:     new Date().toISOString(),
//       isDeleted:     false
//     };

//     if (this.isEditMode) {
//       this.bookService.updateBook({ id: this.editingId, ...bookData }).subscribe(() => {
//         this.showToast('Book updated successfully!', 'success');
//         this.loadBooks();
//         this.closeModal();
//       });
//     } else {
//       this.bookService.createBook(bookData).subscribe(() => {
//         this.showToast('Book created successfully!', 'success');
//         this.loadBooks();
//         this.closeModal();
//       });
//     }
//   }

//   // ── Delete ────────────────────────────────────
//   deleteBook(id: string) {
//     const index = this.books.findIndex(b => b.id === id);
//     if (index !== -1) this.books.splice(index, 1);
//     this.showToast('Book deleted.', 'error');
//     this.bookService.deleteBook(id).subscribe();
//   }

//   // ── Toast ─────────────────────────────────────
//   showToast(message: string, type: string) {
//     this.toastMessage = message;
//     this.toastType    = type;
//     setTimeout(() => { this.toastMessage = ''; }, 2500);
//   }

//   // ── Avatar ────────────────────────────────────
//   getInitial(title: string): string {
//     return title ? title.charAt(0).toUpperCase() : '?';
//   }
// }




// admin/books/books.ts

import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { BookService } from '../../services/book.service';
import { Book } from '../../models/book.model';

import { SearchBarComponent } from '../shared/components/search-bar/search-bar';
import { DataTableComponent } from '../shared/components/data-table/data-table';
import { ModalComponent } from '../shared/components/modal/modal';

@Component({
  selector: 'app-books',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SearchBarComponent,
    DataTableComponent,
    ModalComponent
  ],
  templateUrl: './books.html',
  styleUrl: './books.css'
})
export class BooksComponent implements OnInit {

  private bookService = inject(BookService);

  // ── Options ──────────────────────────────────
  genres = ['Fiction', 'Science', 'Technology', 'History'];
  availability = ['Available', 'Borrowed'];

  // ── Table Columns ─────────────────────────────
  tableColumns = [
    'title',
    'isbn',
    'publishedYear',
    'language'
  ];

  // ── Modal State ───────────────────────────────
  showModal = false;
  showDetailModal = false;

  isEditMode = false;
  editingId = '';

  selectedBook: Book | null = null;

  // ── Toast ─────────────────────────────────────
  toastMessage = '';
  toastType = '';

  // ── Search & Filters ──────────────────────────
  searchText = '';
  filterLanguage = '';
  filterYear = '';

  // ── Form Model ────────────────────────────────
  form = {
    title: '',
    subtitle: '',
    isbn: '',
    edition: '',
    publishedYear: '',
    language: '',
    summary: '',
    coverImageUrl: '',
    subCategoryId: ''
  };

  // ── Books List ────────────────────────────────
  books: Book[] = [];

  // ── Init ──────────────────────────────────────
  ngOnInit() {
    this.loadBooks();
  }

  // ── Load Books ────────────────────────────────
  loadBooks() {
    this.bookService.getBooks().subscribe((data: Book[]) => {
      this.books = data;
    });
  }

  // ── Filter Books ──────────────────────────────
  get filteredBooks(): Book[] {

    return this.books.filter(b => {

      const keyword = this.searchText.toLowerCase();

      const matchSearch =
        !this.searchText ||
        b.title.toLowerCase().includes(keyword) ||
        b.isbn?.toLowerCase().includes(keyword) ||
        b.authors?.some(a => a.toLowerCase().includes(keyword));

      const matchLanguage =
        !this.filterLanguage ||
        b.language?.toLowerCase() === this.filterLanguage.toLowerCase();

      const matchYear =
        !this.filterYear ||
        b.publishedYear?.toString() === this.filterYear;

      return matchSearch && matchLanguage && matchYear;

    });
  }

  // ── Unique Languages ──────────────────────────
  get languages(): string[] {

    const all = this.books
      .map(b => b.language)
      .filter(l => l) as string[];

    return [...new Set(all)];
  }

  // ── Clear Filters ─────────────────────────────
  clearFilters() {
    this.searchText = '';
    this.filterLanguage = '';
    this.filterYear = '';
  }

  // ── View Detail Modal ─────────────────────────
  viewDetail(book: Book) {
    this.selectedBook = book;
    this.showDetailModal = true;
  }

  closeDetailModal() {
    this.showDetailModal = false;
    this.selectedBook = null;
  }

  // ── Open Add Modal ────────────────────────────
  openAddModal() {

    this.isEditMode = false;
    this.editingId = '';

    this.form = {
      title: '',
      subtitle: '',
      isbn: '',
      edition: '',
      publishedYear: '',
      language: '',
      summary: '',
      coverImageUrl: '',
      subCategoryId: ''
    };

    this.showModal = true;
  }

  // ── Open Edit Modal ───────────────────────────
  openEditModal(book: Book) {

    this.isEditMode = true;
    this.editingId = book.id!;

    this.form = {
      title: book.title,
      subtitle: book.subtitle ?? '',
      isbn: book.isbn ?? '',
      edition: book.edition ?? '',
      publishedYear: book.publishedYear?.toString() ?? '',
      language: book.language ?? '',
      summary: book.summary ?? '',
      coverImageUrl: book.coverImageUrl ?? '',
      subCategoryId: book.subCategoryId ?? ''
    };

    this.showModal = true;
  }

  // ── Close Modal ───────────────────────────────
  closeModal() {
    this.showModal = false;
  }

  // ── Save Book ─────────────────────────────────
  saveBook() {

    if (!this.form.title) {
      this.showToast('Please fill in Title!', 'error');
      return;
    }

    const bookData = {
      title: this.form.title,
      subtitle: this.form.subtitle || undefined,
      isbn: this.form.isbn || undefined,
      edition: this.form.edition || undefined,
      publishedYear: this.form.publishedYear
        ? +this.form.publishedYear
        : undefined,
      language: this.form.language || undefined,
      summary: this.form.summary || undefined,
      coverImageUrl: this.form.coverImageUrl || undefined,
      subCategoryId: this.form.subCategoryId,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
      isDeleted: false
    };

    if (this.isEditMode) {

      this.bookService.updateBook({
        id: this.editingId,
        ...bookData
      }).subscribe(() => {

        this.showToast('Book updated successfully!', 'success');

        this.loadBooks();

        this.closeModal();
      });

    } else {

      this.bookService.createBook(bookData).subscribe(() => {

        this.showToast('Book created successfully!', 'success');

        this.loadBooks();

        this.closeModal();
      });

    }
  }

  // ── Delete Book ───────────────────────────────
  deleteBook(id: string) {

    const index = this.books.findIndex(b => b.id === id);

    if (index !== -1) {
      this.books.splice(index, 1);
    }

    this.showToast('Book deleted.', 'error');

    this.bookService.deleteBook(id).subscribe();
  }

  // ── Toast ─────────────────────────────────────
  showToast(message: string, type: string) {

    this.toastMessage = message;
    this.toastType = type;

    setTimeout(() => {
      this.toastMessage = '';
    }, 2500);
  }

  // ── Book Avatar Initial ───────────────────────
  getInitial(title: string): string {
    return title ? title.charAt(0).toUpperCase() : '?';
  }

}