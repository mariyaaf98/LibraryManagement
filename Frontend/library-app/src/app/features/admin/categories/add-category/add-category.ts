import { CommonModule } from "@angular/common";
import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CategoryService } from "../../../../core/services/category.service";
import { ActivatedRoute, Router } from "@angular/router";
import { forkJoin } from "rxjs";

@Component({
  selector: 'app-add-category',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-category.html',
  styleUrls: ['./add-category.css'],
})
export class CategoryFormComponent implements OnInit {

  category = {
    name: '',
    description: '',
    subCategories: [
      { id: '', name: '', description: '' }
    ]
  };

  isEditMode = false;
  categoryId: string | null = null;

  constructor(
    private service: CategoryService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.categoryId = this.route.snapshot.paramMap.get('id');

    if (this.categoryId) {
      this.isEditMode = true;
      this.loadCategory(this.categoryId);
    }
  }

  // LOAD
  loadCategory(id: string) {
    forkJoin({
      categories: this.service.getCategories(),
      subCategories: this.service.getSubCategories()
    }).subscribe({
      next: (res) => {

        const found = res.categories.find(c => c.id === id);

        if (found) {
          this.category.name = found.name;
          this.category.description = found.description || '';
        }

        this.category.subCategories = res.subCategories
          .filter(s => s.categoryId === id)
          .map(s => ({
            id: s.id,
            name: s.name,
            description: s.description || ''
          }));

        if (this.category.subCategories.length === 0) {
          this.category.subCategories = [
            { id: '', name: '', description: '' }
          ];
        }

        this.cdr.detectChanges();
      },
      error: (err: any) => console.error('Load failed:', err)
    });
  }

  // ADD SUB
  addSubCategory() {
    this.category.subCategories.push({
      id: '',
      name: '',
      description: ''
    });
  }

  // REMOVE SUB
  removeSubCategory(index: number) {
    this.category.subCategories =
      this.category.subCategories.filter((_, i) => i !== index);

    if (this.category.subCategories.length === 0) {
      this.category.subCategories = [
        { id: '', name: '', description: '' }
      ];
    }
  }

  // SUBMIT
  submit() {

    if (!this.category.name.trim()) {
      alert('Category name is required');
      return;
    }

    // ===== EDIT =====
    if (this.isEditMode && this.categoryId) {

      this.service.updateCategory(this.categoryId, {
        name: this.category.name,
        description: this.category.description
      }).subscribe({

        next: () => {

          this.service.getSubCategories().subscribe(existingSubs => {

            const existing = existingSubs.filter(s => s.categoryId === this.categoryId);
            const current = this.category.subCategories;

            // CREATE
            const createRequests = current
              .filter(sub => !sub.id && sub.name.trim())
              .map(sub =>
                this.service.createSubCategory({
                  name: sub.name,
                  description: sub.description,
                  categoryId: this.categoryId!
                })
              );

            // DELETE
            const deleteRequests = existing
              .filter(es => !current.some(cs => cs.id === es.id))
              .map(es =>
                this.service.deleteSubCategory(es.id)
              );

            // UPDATE
            const updateRequests = current
              .filter(sub => sub.id)
              .map(sub =>
                this.service.updateSubCategory(sub.id, {   // ✅ FIXED
                  name: sub.name,
                  description: sub.description
                })
              );

            forkJoin([
              ...createRequests,
              ...deleteRequests,
              ...updateRequests
            ]).subscribe({
              next: () => {
                alert('Category & Subcategories updated');
                this.router.navigate(['/admin/category']);
              },
              error: (err: any) => console.error('Subcategory sync failed:', err)
            });

          });

        },

        error: (err: any) => console.error('Update failed:', err)

      });

    }

    // ===== ADD =====
    else {

      this.service.createCategory({
        name: this.category.name,
        description: this.category.description
      }).subscribe({
        next: (created: any) => {

          const categoryId = created.id;

          const requests = this.category.subCategories
            .filter(sub => sub.name.trim())
            .map(sub =>
              this.service.createSubCategory({
                name: sub.name,
                description: sub.description,
                categoryId: categoryId
              })
            );

          if (requests.length > 0) {
            forkJoin(requests).subscribe({
              next: () => {
                alert('Category created with subcategories');
                this.router.navigate(['/admin/category']);
              },
              error: (err: any) => console.error('Subcategory creation failed:', err)
            });
          } else {
            alert('Category created');
            this.router.navigate(['/admin/category']);
          }

          this.cdr.detectChanges();
        },
        error: (err: any) => console.error('Create failed:', err)
      });

    }
  }

  goBack() {
    this.router.navigate(['/admin/category']);
  }
}
