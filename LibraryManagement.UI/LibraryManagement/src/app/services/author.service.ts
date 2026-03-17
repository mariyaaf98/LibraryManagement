// services/author.service.ts
// Same pattern as user.service.ts

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Author } from '../models/author.model';

@Injectable({ providedIn: 'root' })
export class AuthorService {

  private http   = inject(HttpClient);
  private apiUrl = 'http://localhost:5223/api/author';  

  getAuthors(): Observable<Author[]> {
    return this.http.get<Author[]>(this.apiUrl);
  }

  createAuthor(author: Omit<Author, 'id'>): Observable<Author> {
    return this.http.post<Author>(this.apiUrl, author);
  }

  updateAuthor(author: Author): Observable<Author> {
    return this.http.put<Author>(`${this.apiUrl}/${author.id}`, author);
  }

  deleteAuthor(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}