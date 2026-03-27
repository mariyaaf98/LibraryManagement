import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { BookService } from '../../../../core/services/book';
import { CreateBookDto } from '../../../../core/models/book-create.dto';
import { FormsModule } from '@angular/forms';
import { LookupService } from '../../../../core/services/lookup';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-add-book',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-book.html',
  styleUrl: './add-book.css',
})
export class AddBookComponent implements OnInit {

  // Dropdown data
  subCategories: any[] = [];
  authors: any[] = [];
  categories: any[] = [];

  mode: 'add' | 'edit' = 'add';
  bookId!: string;

  // Book model
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

  constructor(
    private bookService: BookService,
    private lookupService: LookupService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}


  // Load dropdown data
  ngOnInit(): void {

    this.loadDropdowns();
    // this.lookupService.getSubCategories()
    //   .subscribe(res => {
    //     this.subCategories = res;
    //   });

    // this.lookupService.getAuthors()
    //   .subscribe(res => {
    //     console.log('Authors:', res);
    //     this.authors = res;
    //   });

    // this.lookupService.getCategories()
    //   .subscribe(res => this.categories = res);

    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.mode = 'edit';
      this.bookId = id;

      this.loadBook(id);
    }
  }


  loadBook(id: string) {
    this.bookService.getBookById(id).subscribe(res => {
      this.book = res;
      this.cdr.detectChanges();
    });
  }

  loadDropdowns() {
    this.lookupService.getSubCategories()
      .subscribe(res => this.subCategories = res);

    this.lookupService.getAuthors()
      .subscribe(res => this.authors = res);

    this.lookupService.getCategories()
      .subscribe(res => this.categories = res);
  }

  selectedFile!: File;

  // select image
  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  isLoading = false;

  // Submit form
  // addBook(form: any) {

  //   if (this.book.publishedYear) {
  //     this.book.publishedYear = Number(this.book.publishedYear);
  //   } else {
  //     this.book.publishedYear = null;
  //   }

  //   if (!this.book.subCategoryId) {
  //     alert('Please select SubCategory');
  //     return;
  //   }

  //   if (!this.selectedFile) {
  //     alert('Please select image');
  //     return;
  //   }

  //   const formData = new FormData();
  //   formData.append('file', this.selectedFile);
  //   formData.append('upload_preset', 'my_preset');

  //   this.bookService.uploadToCloudinary(formData).subscribe((res: any) => {

  //     this.book.coverImageUrl = res.secure_url;
  //     this.isLoading = true;

  //     this.bookService.addBook(this.book).subscribe({
  //       next: response => {
  //         alert('Book saved successfully');

  //         this.isLoading = false;

  //         form.resetForm();


  //         this.router.navigate(['/admin/list-books'], {
  //           state: { refresh: true }
  //         });
  //       },
  //     });

  //   });
  // }



  addBook(form: any) {

    if (this.book.publishedYear) {
      this.book.publishedYear = Number(this.book.publishedYear);
    } else {
      this.book.publishedYear = null;
    }

    if (!this.book.subCategoryId) {
      alert('Please select SubCategory');
      return;
    }

    // ✅ EDIT MODE
    if (this.mode === 'edit') {

      // If new image selected → upload first
      if (this.selectedFile) {

        const formData = new FormData();
        formData.append('file', this.selectedFile);
        formData.append('upload_preset', 'my_preset');

        this.bookService.uploadToCloudinary(formData).subscribe((res: any) => {

          this.book.coverImageUrl = res.secure_url;

          this.updateBook(); // call update
        });

      } else {
        // No new image → just update
        this.updateBook();
      }

      return;
    }

    // ✅ ADD MODE (your existing logic)
    if (!this.selectedFile) {
      alert('Please select image');
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('upload_preset', 'my_preset');

    this.bookService.uploadToCloudinary(formData).subscribe((res: any) => {

      this.book.coverImageUrl = res.secure_url;
      this.isLoading = true;

      this.bookService.addBook(this.book).subscribe({
        next: response => {
          console.log('Saved:', response);
          alert('Book saved successfully');

          this.isLoading = false;

          form.resetForm();

          this.router.navigate(['/admin/list-books'], {
            state: { refresh: true }
          });
        },
        error: err => {
          console.error(err);
          this.isLoading = false;
        }
      });

    });
  }


  updateBook() {
    this.isLoading = true;

    this.bookService.updateBook(this.bookId, this.book).subscribe({
      next: () => {
        alert('Book updated successfully');
        this.isLoading = false;

        this.router.navigate(['/admin/list-books'], {
          state: { refresh: true }
        });
      },
      error: err => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }

}