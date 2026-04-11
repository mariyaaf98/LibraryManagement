import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthorService } from '../../../../../core/services/author.service';
import { AuthorUpdateDto } from '../../../../../core/models/author-update.dto';

@Component({
  selector: 'app-add-author',
  imports: [FormsModule,CommonModule],
  standalone: true,
  templateUrl: './add-author.html',
  styleUrl: './add-author.css',
})
export class AuthorFormComponent implements OnInit {

  mode: 'add' | 'edit' = 'add';
  authorId!: string;

  author = {
    givenName: '',
    familyName: '',
    birthDate: '',
    biography: ''
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authorService: AuthorService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.mode = 'edit';
      this.authorId = id;
      this.loadAuthor(id);
    }
  }

  // Load existing author (for edit)
  loadAuthor(id: string) {
    this.authorService.getAuthorById(id).subscribe(res => {

      const parts = res.fullName?.split(' ') || [];

      this.author = {
        givenName: parts[0] || '',
        familyName: parts.slice(1).join(' ') || '',

        // date input
        birthDate: res.birthDate ? res.birthDate.substring(0, 10) : '',

        biography: ''
      };
      this.cdr.detectChanges();
    });
  }

  // Submit (Add + Edit)
  submit(form: any) {

    if (form.invalid) {
      alert('Please fill required fields');
      return;
    }

    const payload = {
      givenName: this.author.givenName,
      familyName: this.author.familyName,
      biography: this.author.biography,

      birthDate: this.author.birthDate
        ? new Date(this.author.birthDate).toISOString()
        : undefined
    };

    // EDIT
    if (this.mode === 'edit') {

      const updateDto: AuthorUpdateDto = {
        id: this.authorId,
        ...payload
      };

      this.authorService.updateAuthor(this.authorId, updateDto)
        .subscribe(() => {
          alert('Author updated');
          this.cdr.detectChanges();
          this.router.navigate(['/admin/authors']);
        });

    }

    // CREATE
    else {

      this.authorService.createAuthor(payload)
        .subscribe(() => {
          alert('Author created');
          this.cdr.detectChanges();
          this.router.navigate(['/admin/authors']);
        });

    }
  }
}