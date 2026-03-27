import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateBookDto } from '../models/book-create.dto';
import { BookResponseDto } from '../models/book-response.dto';

@Injectable({
  providedIn: 'root',
})

export class BookService {

  private apiUrl = 'http://localhost:5223/api/book';

  constructor(private http: HttpClient) { }

  getBooks(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  addBook(book: CreateBookDto): Observable<any> {
    return this.http.post(this.apiUrl, book);
  }

  deleteBook(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }


  getBookById(id: string): Observable<CreateBookDto> {
    return this.http.get<CreateBookDto>(`${this.apiUrl}/${id}`);
  }

  updateBook(id: string, book: CreateBookDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, book);
  }

  // Search + Filter
  searchBooks(
    search?: string,
    genre?: string,
    status?: string
  ): Observable<BookResponseDto[]> {

    let params = new HttpParams();

    if (search) {
      params = params.set('search', search);
    }

    if (genre) {
      params = params.set('genre', genre);
    }

    if (status) {
      params = params.set('status', status);
    }

    return this.http.get<BookResponseDto[]>(
      `${this.apiUrl}/search`,
      { params }
    );
  }

  // upload image to Cloudinary
  uploadToCloudinary(formData: FormData) {
    return this.http.post(
      'https://api.cloudinary.com/v1_1/dvblzijuc/image/upload',
      formData
    );
  }
}
