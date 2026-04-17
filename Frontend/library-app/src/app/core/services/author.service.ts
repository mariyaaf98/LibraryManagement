import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AuthorResponseDto } from '../models/author-response.dto';
import { AuthorCreateDto } from '../models/author-create.dto';


@Injectable({
  providedIn: 'root'
})
export class AuthorService {

  private apiUrl = 'http://localhost:5223/api/Author';

  constructor(private http: HttpClient) {}

  // GET ALL
  getAuthors(): Observable<AuthorResponseDto[]> {
    return this.http.get<AuthorResponseDto[]>(this.apiUrl);
  }

  // GET BY ID
  getAuthorById(id: string): Observable<AuthorResponseDto> {
    return this.http.get<AuthorResponseDto>(`${this.apiUrl}/${id}`);
  }

  // CREATE
  createAuthor(dto: AuthorCreateDto): Observable<AuthorResponseDto> {
    return this.http.post<AuthorResponseDto>(this.apiUrl, dto);
  }

  // UPDATE
  updateAuthor(id: string, dto: AuthorCreateDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  // DELETE (Soft Delete)
  deleteAuthor(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }


  getBooksByAuthor(id: string) {
  return this.http.get<any[]>(`${this.apiUrl}/${id}/books`);

}

}
