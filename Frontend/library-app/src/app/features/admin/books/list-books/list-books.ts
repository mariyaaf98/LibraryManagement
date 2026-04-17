import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { BookService } from '../../../../core/services/book';
import { CopyService } from '../../../../core/services/copy.service';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

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
    private copyService: CopyService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {

    // Reload books whenever navigation happens
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.loadBooks();
      });
  }

  ngOnInit(): void {
    this.loadBooks();
  }

  // Load books + calculate availableCopies
  loadBooks() {
    this.isLoading = true;

    this.bookService.getBooks().subscribe({
      next: (booksRes) => {

        this.copyService.getAll().subscribe({
          next: (copiesRes) => {

            this.books = booksRes.map((book: any) => {

              const availableCopies = copiesRes.filter(
                (copy: any) =>
                  copy.bookId === book.id &&
                  copy.status === 'AVAILABLE'
              ).length;

              return {
                ...book,
                availableCopies
              };
            });

            this.isLoading = false;

            this.cdr.detectChanges();
          },
          //Error Handling (Copies API),Prevent app crash & stop loading
          error: (err) => {
            console.error('Error loading copies:', err);
            this.isLoading = false;
            this.cdr.detectChanges();
          }
        });

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

  // Navigate to Copies
  goToCopies(bookId: string) {
    this.router.navigate(['/admin/copies', bookId]);
  }

}
