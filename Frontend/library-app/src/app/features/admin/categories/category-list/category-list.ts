import { CommonModule } from "@angular/common";
import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { CategoryResponseDto } from "../../../../core/models/category-response.dto";
import { SubCategoryResponseDto } from "../../../../core/models/subcategory-response.dto";
import { CategoryService } from "../../../../core/services/category.service";
import { Router } from "@angular/router";
import { forkJoin } from "rxjs";

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './category-list.html',
  styleUrls: ['./category-list.css'],
})
export class CategoryListComponent implements OnInit {

  categories: CategoryResponseDto[] = [];
  subCategories: SubCategoryResponseDto[] = [];
  isLoading = true;

  constructor(
    private service: CategoryService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  // ───── LOAD DATA ─────
  loadData() {
    this.isLoading = true;

    forkJoin({
      categories: this.service.getCategories(),
      subCategories: this.service.getSubCategories()
    }).subscribe({
      next: (res) => {
        this.categories = res.categories;
        this.subCategories = res.subCategories;
        this.isLoading = false;

        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Load failed:', err);
        this.isLoading = false;

        this.cdr.detectChanges();
      }
    });
  }

  // ───── GET SUBCATEGORIES BY CATEGORY ─────
  getSubCategories(categoryId: string): SubCategoryResponseDto[] {
    return this.subCategories.filter(s => s.categoryId === categoryId);
  }

  // ───── NAVIGATE EDIT ─────
  editCategory(id: string) {
    this.router.navigate(['/admin/category/edit', id]);
  }

  // ───── DELETE CATEGORY ─────
  confirmDelete(id: string) {
    if (confirm('Delete this category?')) {

      this.service.deleteCategory(id).subscribe({
        next: () => {

          this.categories = this.categories.filter(c => c.id !== id);
          this.subCategories = this.subCategories.filter(s => s.categoryId !== id);
          this.cdr.detectChanges();
          
        },
        error: (err) => {
          console.error('Delete failed:', err);
        }
      });
    }
  }

  // ───── NAVIGATE CREATE ─────
  goToAddCategory() {
    this.router.navigate(['/admin/category/create']);
  }
}
