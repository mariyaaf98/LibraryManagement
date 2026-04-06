import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BookService } from '../../../../core/services/book';
import { BookResponseDto } from '../../../../core/models/book-response.dto';
import { NgClass } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-catalogue',
  standalone: true,
  imports: [FormsModule,NgClass],
  templateUrl: './catalogue.html',
  styleUrl: './catalogue.css',
})
export class CatalogueComponent implements OnInit {

  filteredBooks: BookResponseDto[] = [];
  genres: string[] = [];
  searchText: string = '';
  selectedStatus: string = '';
  selectedGenre: string = '';

  constructor(
    private bookService: BookService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadBooks();
  }

  //  Load from backend
  loadBooks() {
    this.bookService
      .searchBooks(this.searchText, this.selectedGenre, this.selectedStatus)
      .subscribe(data => {
        this.filteredBooks = data;

        //Extract unique category
        this.genres = [...new Set(data.map(b => b.subCategoryName || ''))];
        this.cdr.detectChanges();
      });
  }

  // Search
  onSearch() {
    this.loadBooks();
  }

  //  Status filter
  setStatus(status: string) {
    this.selectedStatus = status;
    this.loadBooks();
  }

  setGenre(genre: string) {
    this.selectedGenre = genre;
    this.loadBooks();
  }


  openBookDetails(id: string) {
  this.router.navigate(['/member/book', id]);
  
}
}