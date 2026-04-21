import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { BookService } from '../../../../core/services/book';
import { CreateBookDto } from '../../../../core/models/book-create.dto';
import { FormsModule, NgForm } from '@angular/forms';
import { LookupService } from '../../../../core/services/lookup';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-book',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-book.html',
  styleUrl: './add-book.css',
})
export class AddBookComponent implements OnInit {

  subCategories: { id: string; name: string }[] = [];
  authors: { id: string; fullName: string }[] = [];
  categories: { id: string; name: string }[] = [];
  currentYear = new Date().getFullYear();

  mode: 'add' | 'edit' = 'add';
  bookId!: string;

  book: CreateBookDto = {
    title: '',
    subtitle: '',
    isbn: '',
    edition: '',
    publishedYear: null,
    language: '',
    summary: '',
    coverImageUrl: '',
    subCategoryId: '',
    authorIds: [],
    categoryIds: []
  };

  selectedFile!: File;
  fileError: string = '';
  isLoading: boolean = false;

  constructor(
    private bookService: BookService,
    private lookupService: LookupService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.mode = 'edit';
      this.bookId = id;
    }

    this.loadDropdowns(() => {
      if (this.mode === 'edit') {
        this.loadBook(this.bookId);
      }
    });

  }

  loadDropdowns(callback?: () => void) {
    this.lookupService.getSubCategories().subscribe(sub => {
      this.subCategories = sub;

      this.lookupService.getAuthors().subscribe(auth => {
        this.authors = auth;

        this.lookupService.getCategories().subscribe(cat => {
          this.categories = cat;

          if (callback) callback();
          this.cdr.detectChanges();
        });
      });
    });

  }

  loadBook(id: string) {
    this.bookService.getBookById(id).subscribe(res => {

      const authorIds = this.authors
        .filter(a => res.authors.some(r => r.id === a.id))
        .map(a => a.id);

      const categoryIds = this.categories
        .filter(c => res.categories.includes(c.name))
        .map(c => c.id);

      const subCategoryId = this.subCategories.find(
        s => s.name === res.subCategory?.name
      )?.id || '';

      this.book = {
        title: res.title,
        subtitle: '',
        isbn: res.isbn || '',
        edition: '',
        publishedYear: res.publishedYear ?? null,
        language: res.language || '',
        summary: res.summary || '',
        coverImageUrl: res.coverImageUrl || '',
        subCategoryId,
        authorIds,
        categoryIds
      };

      this.cdr.detectChanges();
    });

  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    this.fileError = '';

    if (!file) return;

    if (!file.type.startsWith('image/')) {
      this.fileError = 'Only image files allowed';
      return;
    }

    this.selectedFile = file;
    this.cdr.detectChanges();

  }

  // =========================
  // MAIN SUBMIT FUNCTION
  // =========================
  addBook(form: NgForm) {
    console.log('FINAL BOOK:', this.book);

    // Step 1: mark all fields touched
    if (form.invalid) {
      Object.values(form.controls).forEach((c: any) => c.markAsTouched());
    }

    // Step 2: manual validation (IMPORTANT)
    if (!this.book.title || this.book.title.trim().length < 2) {
      alert('Title is required (min 2 chars)');
      return;
    }

    if (!this.book.subCategoryId) {
      alert('Select subcategory');
      return;
    }

    if (!this.book.language) {
      alert('Select language');
      return;
    }

    if (!this.book.categoryIds || this.book.categoryIds.length === 0) {
      alert('Select at least one category');
      return;
    }

    if (!this.book.authorIds || this.book.authorIds.length === 0) {
      alert('Select at least one author');
      return;
    }

    if (this.mode === 'add' && !this.selectedFile) {
      this.fileError = 'Cover image is required';
      return;
    }

    // Step 3: convert year 
    const year: any = this.book.publishedYear;

    if (year === '' || year === undefined || year === null) {
      this.book.publishedYear = null;
    } else {
      this.book.publishedYear = Number(year);
    }

    // Step 4: edit or add
    if (this.mode === 'edit') {
      this.selectedFile ? this.uploadAndUpdate() : this.updateBook();
      return;
    }

    // Step 5: create
    this.uploadAndCreate(form);

  }

  uploadAndCreate(form: NgForm) {
    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('upload_preset', 'my_preset');

    this.bookService.uploadToCloudinary(formData).subscribe((res: any) => {

      this.book.coverImageUrl = res.secure_url;
      this.isLoading = true;

      this.bookService.addBook(this.book).subscribe({
        next: () => {
          alert('Book saved successfully');
          this.isLoading = false;
          form.resetForm();
          this.router.navigate(['/admin/list-books']);
        },
        error: err => {
          console.log('FULL ERROR:', err);

          alert(err.error?.message || 'Something went wrong');

          this.isLoading = false;
        }
      });

      this.cdr.detectChanges();
    });

  }

  uploadAndUpdate() {
    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('upload_preset', 'my_preset');

    this.bookService.uploadToCloudinary(formData).subscribe((res: any) => {
      this.book.coverImageUrl = res.secure_url;
      this.updateBook();
      this.cdr.detectChanges();
    });

  }

  updateBook() {
    this.isLoading = true;

    this.bookService.updateBook(this.bookId, this.book).subscribe({
      next: () => {
        alert('Book updated successfully');
        this.isLoading = false;
        this.router.navigate(['/admin/list-books']);
      },
      error: err => {
        console.error(err);
        this.isLoading = false;
      }
    });

    this.cdr.detectChanges();

  }
}