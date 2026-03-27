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
          import('./features/admin/pages/add-book/add-book')
            .then(m => m.AddBookComponent)
      },
      {
        path: 'edit-book/:id',
        loadComponent: () =>
          import('./features/admin/pages/add-book/add-book')
            .then(m => m.AddBookComponent)
      },
      {
        path: 'list-books',
        loadComponent: () =>
          import('./features/admin/pages/list-books/list-books')
            .then(m => m.ListBooksComponent)
      }
    ]
  },

  { path: '', redirectTo: 'member', pathMatch: 'full' }
];