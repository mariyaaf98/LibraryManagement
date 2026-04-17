import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { AuthorResponseDto } from '../../../../../core/models/author-response.dto';
import { AuthorService } from '../../../../../core/services/author.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-author-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './author-list.html',
  styleUrls: ['./author-list.css'],
})
export class AuthorListComponent implements OnInit {

  @Input() isReadOnly: boolean = false;
  authors: AuthorResponseDto[] = [];

  constructor(
    private authorService: AuthorService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef 
  ) {}

  ngOnInit(): void {

    //  read from route
    this.isReadOnly = this.route.snapshot.data['readOnly'] ?? false;

    this.loadAuthors();
  }

  loadAuthors() {
    this.authorService.getAuthors().subscribe({
      next: (res) => {
        this.authors = res || [];

        this.cdr.detectChanges();
      },
      error: (err) => console.error(err)
    });
  }

  goToCreate() {
    if (this.isReadOnly) return;
    this.router.navigate(['/admin/authors/create']);
  }

  goToEdit(id: string) {
    if (this.isReadOnly) return;
    this.router.navigate(['/admin/authors/edit', id]);
  }

  deleteAuthor(id: string) {
    if (this.isReadOnly) return;

    if (!confirm('Are you sure you want to delete this author?')) return;

    this.authorService.deleteAuthor(id).subscribe({
      next: () => {
        this.loadAuthors();

  
        this.cdr.detectChanges();
      },
      error: (err) => console.error(err)
    });
  }

  goToDetails(id: string) {
  if (!this.isReadOnly) return; // only for member
  this.router.navigate(['/member/authors', id]);
}
}