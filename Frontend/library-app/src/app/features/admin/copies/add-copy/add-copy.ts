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

  bookId!: string;
  copyId!: string;
  isEditMode = false;

  copy: CreateCopy = {
    bookId: '',
    barcode: '',
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
  ) {}

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
            barcode: existing.barcode,
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

  // save(): void {

  //   if (!this.copy.barcode?.trim()) {
  //     alert('Barcode is required');
  //     return;
  //   }

  //   if (this.isEditMode) {

  //     this.service.update(this.copyId, this.copy).subscribe({
  //       next: () => {
  //         this.router.navigate(['/admin/copies', this.bookId]);
  //       },
  //       error: (err: any) => {
  //         console.error(err);
  //       }
  //     });

  //   } else {

  //     this.service.create(this.copy).subscribe({
  //       next: () => {
  //         this.router.navigate(['/admin/copies', this.bookId]);
  //       },
  //       error: (err: any) => {
  //         console.error(err);
  //       }
  //     });

  //   }
  // }



  save(): void {

  if (!this.copy.barcode?.trim()) {
    alert('Barcode is required');
    return;
  }

  const request = this.isEditMode
    ? this.service.update(this.copyId, this.copy)
    : this.service.create(this.copy);

  request.subscribe({
    next: () => {

      // 🔥 Send refresh flag
      this.router.navigate(
        ['/admin/copies', this.bookId],
        { state: { refresh: true } }
      );

    },
    error: (err: any) => {
      console.error(err);
    }
  });
}

  goBack(): void {
    this.router.navigate(['/admin/copies', this.bookId]);
  }
}