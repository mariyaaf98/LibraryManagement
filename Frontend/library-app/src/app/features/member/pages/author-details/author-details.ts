import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AuthorResponseDto } from '../../../../core/models/author-response.dto';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorService } from '../../../../core/services/author.service';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-author-details',
  imports: [CommonModule, DatePipe],
  templateUrl: './author-details.html',
  styleUrl: './author-details.css',
})
export class AuthorDetailsComponent implements OnInit {

  author?: AuthorResponseDto;
  loading = true;
  books: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private authorService: AuthorService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;

    this.loadAuthor(id);
    this.loadBooks(id);
  }

  loadAuthor(id: string) {
    this.authorService.getAuthorById(id).subscribe({
      next: (res) => {
        this.author = res;
        this.loading = false;

        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        this.loading = false;

        this.cdr.detectChanges();
      }
    });
  }

  loadBooks(id: string) {
    this.authorService.getBooksByAuthor(id).subscribe({
      next: (res) => {
        console.log('Books:', res);
        this.books = res;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error loading books:', err);
      }
    });
  }

  goToBook(id: string) {
     this.router.navigate(['/member/book', id]);
  }
}
