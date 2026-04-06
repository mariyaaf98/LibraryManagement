import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CopyService } from '../../../../core/services/copy.service';
import { Copy } from '../../../../core/models/copy-response.dto';

@Component({
  selector: 'app-copy-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './copy-list.html',
  styleUrls: ['./copy-list.css'],
})
export class CopyListComponent implements OnInit {

  copies: Copy[] = [];
  bookId!: string;
  loading = true;
  currentCopyId?: string;

  constructor(
    private route: ActivatedRoute,
    private copyService: CopyService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('bookId');

      if (!id) {
        console.error('No Book ID in route');
        this.loading = false;
        this.cdr.detectChanges();
        return;
      }

      this.bookId = id;
      this.loadCopies();
    });
  }

  // 🔹 Load copies
  loadCopies(): void {
    this.loading = true;

    this.copyService.getAll().subscribe({
      next: (res) => {

        this.copies = res.filter(c => {
          const dbId = c.bookId?.trim().toLowerCase();
          const routeId = this.bookId?.trim().toLowerCase();
          return dbId === routeId;
        });

        this.updateCurrentCopy();

        this.loading = false;
        this.cdr.detectChanges();
      },

      error: (err) => {
        console.error('API ERROR:', err);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  // 🔹 Total copies
  getTotalCopies(): number {
    return this.copies.length;
  }

  // 🔹 Available copies
  getAvailableCount(): number {
    return this.copies.filter(
      c => c.status?.toUpperCase() === 'AVAILABLE'
    ).length;
  }

  // 🔹 Checked out copies
  getCheckedOutCount(): number {
    return this.copies.filter(
      c => c.status?.toUpperCase() !== 'AVAILABLE'
    ).length;
  }

  // 🔹 Navigate to Add Copy
  goToAddCopy(): void {
    this.router.navigate(['/admin/add-copy', this.bookId]);
  }

  // 🔹 Copy number
  getCopyNumber(copy: Copy): number {
    const sorted = [...this.copies].sort((a, b) =>
      new Date(b.acquisitionDate!).getTime() -
      new Date(a.acquisitionDate!).getTime()
    );

    return sorted.findIndex(c => c.id === copy.id) + 1;
  }

  // 🔹 Edit
  editCopy(copyId: string): void {
    this.router.navigate(['/admin/edit-copy', copyId]);
  }

  // 🔹 Delete
  deleteCopy(copyId: string): void {
    const confirmDelete = confirm('Are you sure you want to delete this copy?');
    if (!confirmDelete) return;

    this.copyService.delete(copyId).subscribe({
      next: () => {
        this.copies = this.copies.filter(c => c.id !== copyId);

        // ✅ Fix: now method exists
        this.updateCurrentCopy();

        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Delete failed:', err);
      }
    });
  }

  // 🔥 IMPORTANT (Missing before)
  updateCurrentCopy(): void {

    const availableCopy = this.copies.find(
      c => c.status?.toUpperCase() === 'AVAILABLE'
    );

    const latestCopy = [...this.copies].sort((a, b) =>
      new Date(b.acquisitionDate!).getTime() -
      new Date(a.acquisitionDate!).getTime()
    )[0];

    this.currentCopyId = availableCopy?.id || latestCopy?.id;
  }
}