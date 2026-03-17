
import { Routes } from '@angular/router';
import { DashboardComponent }  from './admin/dashboard/dashboard';
import { UsersComponent }      from './admin/users/users';
import { AuthorsComponent }    from './admin/authors/authors';  
import { BooksComponent } from './admin/books/books';
     
          

export const routes: Routes = [
  { path: 'admin/dashboard',  component: DashboardComponent  },
  { path: 'admin/users',      component: UsersComponent      },
  { path: 'admin/authors',    component: AuthorsComponent    },  
  { path: 'admin/books',      component: BooksComponent      },
  
  { path: '', redirectTo: 'admin/dashboard', pathMatch: 'full' },
];