import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BookService } from '../../../../core/services/book';
import { BookResponseDto } from '../../../../core/models/book-response.dto';
import { NgClass } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-catalogue',
  standalone: true,
  imports: [FormsModule, NgClass],
  templateUrl: './catalogue.html',
  styleUrl: './catalogue.css',
})
export class CatalogueComponent implements OnInit {

  allBooks: BookResponseDto[] = [];
  filteredBooks: BookResponseDto[] = [];

  genres: string[] = [];

  searchText: string = '';
  selectedStatus: string = '';
  selectedGenre: string = '';

  constructor(
    private bookService: BookService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadBooks();
  }

  // Load data
  loadBooks() {
    this.bookService.searchBooks('', '', '').subscribe({
      next: (data) => {
        console.log('API Response:', data);

        this.allBooks = data || [];
        this.filteredBooks = [...this.allBooks];

        // Extract genres safely
        this.genres = [
          ...new Set(
            this.allBooks
              .map(b => b.subCategory?.name || (b as any).subCategoryName)
              .filter(g => g)
          )
        ];

        // Trigger UI update
        this.cdr.detectChanges();
      },
      error: (err) => console.error(err)
    });
  }

  //Apply filter
  applyFilter() {

    const search = (this.searchText || '').toLowerCase();


    this.filteredBooks = this.allBooks.filter(book => {
      const copies = book.totalCopies ?? 0;

      const matchSearch =
        !this.searchText ||
        book.title?.toLowerCase().includes(search) ||
        book.isbn?.toLowerCase().includes(search) ||
        book.authors?.some(a =>
          a.fullName.toLowerCase().includes(search)
        );

      const bookGenre =
        book.subCategory?.name || (book as any).subCategoryName;

      const matchGenre =
        !this.selectedGenre || bookGenre === this.selectedGenre;

      const matchStatus =
        !this.selectedStatus ||

        // Available = ANY book with copies
        (this.selectedStatus === 'available' && copies > 0) ||

        // Low stock
        (this.selectedStatus === 'low' && copies > 0 && copies <= 3) ||

        // Out of stock
        (this.selectedStatus === 'out' && copies === 0);

      return matchSearch && matchGenre && matchStatus;
    });

    // Trigger UI update
    this.cdr.detectChanges();
  }

  getBookStatus(book: BookResponseDto): string {
    const copies = book.totalCopies ?? 0;

    if (copies === 0) return 'out';
    if (copies <= 2) return 'low';
    return 'available';
  }

  // Search
  onSearch() {
    this.applyFilter();
  }

  // Genre
  setGenre(genre: string) {
    this.selectedGenre = genre;
    this.applyFilter();

    // Optional (extra safe)
    this.cdr.detectChanges();
  }

  // Status

  setStatus(status: string) {
    console.log('Selected:', status);
    this.selectedStatus = status.toLowerCase(); 
    this.applyFilter();
    this.cdr.detectChanges();
  }

  // Navigation
  openBookDetails(id: string) {
    this.router.navigate(['/member/book', id]);
  }
}
