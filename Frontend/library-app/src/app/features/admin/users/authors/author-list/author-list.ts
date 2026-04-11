import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AuthorResponseDto } from '../../../../../core/models/author-response.dto';
import { AuthorService } from '../../../../../core/services/author.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-author-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './author-list.html',
  styleUrls: ['./author-list.css'],
})
export class AuthorListComponent implements OnInit {

  authors: AuthorResponseDto[] = [];

  constructor(
    private authorService: AuthorService,
    private router: Router,
    private cdr: ChangeDetectorRef   
  ) {}

  ngOnInit(): void {
    this.loadAuthors();
  }

  loadAuthors() {
    this.authorService.getAuthors().subscribe({
      next: (res) => {
        this.authors = res;
        this.cdr.detectChanges();  
      },
      error: (err) => console.error(err)
    });
  }

  goToCreate() {
    this.router.navigate(['/admin/authors/create']);
  }

  goToEdit(id: string) {
    this.router.navigate(['/admin/authors/edit', id]);
  }

  deleteAuthor(id: string) {
    if (!confirm('Are you sure you want to delete this author?')) return;

    this.authorService.deleteAuthor(id).subscribe(() => {
      this.loadAuthors();
      this.cdr.detectChanges();   
    });
  }
}