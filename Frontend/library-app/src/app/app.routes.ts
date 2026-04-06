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
          import('./features/member/pages/overview/overview')
            .then(m => m.OverviewComponent)
      },
      {
        path: 'catalogue',
        loadComponent: () =>
          import('./features/member/pages/catalogue/catalogue')
            .then(m => m.CatalogueComponent)
      },
      {
        path: 'book/:id',
        loadComponent: () =>
          import('./features/member/pages/book-details/book-details')
            .then(m => m.BookDetailsComponent)
      }
    ]
  },

  {
    path: 'admin',
    component: AdminLayoutComponent,
    children: [
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
          import('./features/admin/users/user-list/user-list')
            .then(m => m.UserListComponent)
      }
    ]
  },

  { path: '', redirectTo: 'member', pathMatch: 'full' }
];