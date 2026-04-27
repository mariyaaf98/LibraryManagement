import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CreateCopy } from '../../../../core/models/copy-create.dto';
import { CopyService } from '../../../../core/services/copy.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-copy',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-copy.html',
  styleUrls: ['./add-copy.css'],
})
export class AddCopyComponent implements OnInit {

  generatedBarcode: string = '';
  bookId!: string;
  copyId!: string;
  isEditMode = false;

  copy: CreateCopy = {
    bookId: '',
    acquisitionDate: '',
    location: '',
    status: 'AVAILABLE',
    condition: 'GOOD'
  };

  constructor(
    private route: ActivatedRoute,
    private service: CopyService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {

    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.isEditMode = true;
      this.copyId = id;

      this.service.getAll().subscribe(res => {
        const existing = res.find(c => c.id === this.copyId);

        if (existing) {
          this.copy = {
            bookId: existing.bookId,
            acquisitionDate: existing.acquisitionDate
              ? existing.acquisitionDate.split('T')[0]
              : '',
            location: existing.location || '',
            status: existing.status,
            condition: existing.condition
          };

          this.bookId = existing.bookId;

          this.cdr.detectChanges();
        }
      });

    } else {
      this.bookId = this.route.snapshot.paramMap.get('bookId')!;
      this.copy.bookId = this.bookId;

      this.copy.acquisitionDate =
        new Date().toISOString().split('T')[0];

      this.cdr.detectChanges(); // optional
    }
  }


  save(): void {


    const request = this.isEditMode
      ? this.service.update(this.copyId, this.copy)
      : this.service.create(this.copy);

    request.subscribe({
      next: (res: any) => {

        if (!this.isEditMode) {
          this.generatedBarcode = res.barcode;
          alert('Generated Barcode: ' + res.barcode);

          // ⏳ Delay navigation so user sees it
          setTimeout(() => {
            this.router.navigate(
              ['/admin/copies', this.bookId],
              { state: { refresh: true } }
            );
          }, 1000);

          return;
        }

        this.router.navigate(
          ['/admin/copies', this.bookId],
          { state: { refresh: true } }
        );
      },
      error: (err: any) => {
        console.error(err);
        alert(err.error?.message || 'Something went wrong');
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/admin/copies', this.bookId]);
  }
}