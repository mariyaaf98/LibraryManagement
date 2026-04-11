import { Routes } from '@angular/router';
import { MemberLayoutComponent } from './layout/member-layout/member-layout';
import { AdminLayoutComponent } from './layout/admin-layout/admin-layout';


export const routes: Routes = [

  {
    path: 'member',
    component: MemberLayoutComponent,
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./features/member/pages/catalogue/catalogue')
            .then(m => m.CatalogueComponent)
      },
      {
        path: 'book/:id',
        loadComponent: () =>
          import('./features/member/pages/book-details/book-details')
            .then(m => m.BookDetailsComponent)
      },

      {
        path: 'authors/:id',
        loadComponent: () =>
          import('./features/member/pages/author-details/author-details')
            .then(m => m.AuthorDetailsComponent)
      }
    ]
  },

  {

    path: 'admin',
    component: AdminLayoutComponent,
    children: [

      {
        path: '',
        loadComponent: () =>
          import('./features/admin/dashboard/dashboard')
            .then(m => m.AdminDashboardComponent)
      },
      {
        path: 'add-book',
        loadComponent: () =>
          import('./features/admin/books/add-book/add-book')
            .then(m => m.AddBookComponent)
      },
      {
        path: 'edit-book/:id',
        loadComponent: () =>
          import('./features/admin/books/add-book/add-book')
            .then(m => m.AddBookComponent)
      },
      {
        path: 'list-books',
        loadComponent: () =>
          import('./features/admin/books/list-books/list-books')
            .then(m => m.ListBooksComponent)
      },
      {
        path: 'copies/:bookId',
        loadComponent: () =>
          import('./features/admin/copies/copy-list/copy-list')
            .then(m => m.CopyListComponent)
      },


      {
        path: 'add-copy/:bookId',
        loadComponent: () =>
          import('./features/admin/copies/add-copy/add-copy')
            .then(m => m.AddCopyComponent)
      },
      {
        path: 'edit-copy/:id',
        loadComponent: () =>
          import('./features/admin/copies/add-copy/add-copy')
            .then(m => m.AddCopyComponent)
      },
      {
        path: 'users',
        loadComponent: () =>
          import('./features/admin/users/user/user-list/user-list')
            .then(m => m.UserListComponent)
      },
      {
        path: 'users/create',
        loadComponent: () =>
          import('./features/admin/users/user/add-user/add-user')
            .then(m => m.UserFormComponent)
      },

      {
        path: 'users/edit/:id',
        loadComponent: () =>
          import('./features/admin/users/user/add-user/add-user')
            .then(m => m.UserFormComponent)
      },

      {
        path: 'authors',
        loadComponent: () =>
          import('./features/admin/users/authors/author-list/author-list')
            .then(m => m.AuthorListComponent)
      },
      {
        path: 'authors/create',
        loadComponent: () =>
          import('./features/admin/users/authors/add-author/add-author')
            .then(m => m.AuthorFormComponent)
      },
      {
        path: 'authors/edit/:id',
        loadComponent: () =>
          import('./features/admin/users/authors/add-author/add-author')
            .then(m => m.AuthorFormComponent)
      },
      {
        path: 'category',
        loadComponent: () =>
          import('./features/admin/categories/category-list/category-list')
            .then(m => m.CategoryListComponent)
      },
      {
        path: 'category/create',
        loadComponent: () =>
          import('./features/admin/categories/add-category/add-category')
            .then(m => m.CategoryFormComponent)
      },
      {
        path: 'category/edit/:id',
        loadComponent: () =>
          import('./features/admin/categories/add-category/add-category')
            .then(m => m.CategoryFormComponent)
      }
    ]
  },

  { path: '', redirectTo: 'member', pathMatch: 'full' }
];