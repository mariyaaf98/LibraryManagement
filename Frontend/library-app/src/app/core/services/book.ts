import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { CreateBookDto } from '../models/book-create.dto';
import { BookResponseDto } from '../models/book-response.dto';

@Injectable({
  providedIn: 'root',
})
export class BookService {

  private apiUrl = 'http://localhost:5223/api/book';

  
  constructor(private http: HttpClient) {}

  // GET ALL BOOKS
  getBooks(): Observable<BookResponseDto[]> {
    return this.http.get<BookResponseDto[]>(this.apiUrl);
  }

  // GET BOOK BY ID
  getBookById(id: string): Observable<BookResponseDto> {
    return this.http.get<BookResponseDto>(`${this.apiUrl}/${id}`);
  }

  // ADD BOOK
  addBook(book: CreateBookDto): Observable<void> {
    return this.http.post<void>(this.apiUrl, book);
  }

  // UPDATE BOOK
  updateBook(id: string, book: CreateBookDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, book);
  }

  // DELETE BOOK
  deleteBook(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // SEARCH + FILTER
  searchBooks(
    search?: string,
    genre?: string,
    status?: string
  ): Observable<BookResponseDto[]> {

    let params = new HttpParams();

    if (search) params = params.set('search', search);
    if (genre) params = params.set('genre', genre);
    if (status) params = params.set('status', status);

    return this.http.get<BookResponseDto[]>(
      `${this.apiUrl}/search`,
      { params }
    );
  }

  // CLOUDINARY IMAGE UPLOAD
  uploadToCloudinary(formData: FormData): Observable<any> {
    return this.http.post(
      'https://api.cloudinary.com/v1_1/dvblzijuc/image/upload',
      formData
    );
  }

  // // CENTRAL ERROR HANDLER
  // private handleError(error: HttpErrorResponse) {

  //   console.error('Backend Error:', error);

  //   let message = 'Something went wrong';

  //   if (error.error?.message) {
  //     message = error.error.message;
  //   }

  //   return throwError(() => new Error(message));
  // }
}