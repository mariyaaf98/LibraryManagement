import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { BookService } from '../../../../core/services/book';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-books',
  standalone: true,
  templateUrl: './list-books.html',
  styleUrls: ['./list-books.css'],
})
export class ListBooksComponent implements OnInit {

  books: any[] = [];
  isLoading = true;

  constructor(
    private bookService: BookService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadBooks();
  }

  // Load books with change detection
  loadBooks() {
    this.isLoading = true;

    this.bookService.getBooks().subscribe({
      next: (res) => {
        this.books = [...res]; 
        this.isLoading = false;

        this.cdr.detectChanges(); 
      },
      error: (err) => {
        console.error('Error loading books:', err);
        this.isLoading = false;

        this.cdr.detectChanges(); 
      }
    });
  }

  // Delete book
  deleteBook(id: string) {
    this.bookService.deleteBook(id).subscribe({
      next: () => {
        this.books = this.books.filter(book => book.id !== id);

        this.cdr.detectChanges(); 
      },
      error: (err) => {
        console.error('Delete failed:', err);
      }
    });
  }

  // Confirm before delete
  confirmDelete(id: string) {
    if (confirm('Delete this book?')) {
      this.deleteBook(id);
    }
  }

  // Navigate to Add
  goToAddBook() {
    this.router.navigate(['/admin/add-book']);
  }

  // Navigate to Edit
  editBook(id: string) {
    this.router.navigate(['/admin/edit-book', id]);
  }
}