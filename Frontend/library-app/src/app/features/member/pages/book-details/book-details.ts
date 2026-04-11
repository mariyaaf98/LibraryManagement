import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BookService } from '../../../../core/services/book';
import { BookResponseDto } from '../../../../core/models/book-response.dto';

@Component({
  selector: 'app-book-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './book-details.html',
  styleUrl: './book-details.css',
})
export class BookDetailsComponent implements OnInit {

  book: BookResponseDto | null = null;

  activeTab: 'description' | 'copies' = 'description';

  bookId!: string;

  isLoading = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private bookService: BookService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.errorMessage = 'Book ID is missing';
      return;
    }

    this.bookId = id;
    this.loadBook(this.bookId);
  }

  loadBook(id: string) {
    this.isLoading = true;

    this.bookService.getBookById(id).subscribe({
      next: (res) => {

        console.log('BOOK DETAILS:', res);

        this.book = res;

        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Failed to load book details';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  goToAuthorDetails(id: string | undefined) {

  console.log('Clicked author id:', id); //

  if (!id) {
    console.error('Author ID is undefined!');
    return;
  }

  this.router.navigate(['/member/authors', id]);
}

  setTab(tab: 'description' | 'copies') {
    this.activeTab = tab;
  }

  borrowBook() {
    if (!this.book) return;

    if (this.book.totalCopies === 0) {
      alert('No copies available');
      return;
    }

    console.log('Borrow Book:', this.bookId);
  }

  goBack() {
    this.router.navigate(['/member']);
  }
}