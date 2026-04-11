import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root',
})
export class LookupService {
  createAuthor(arg0: { fullName: string; }) {
    throw new Error('Method not implemented.');
  }

  private http = inject(HttpClient);

  private baseUrl = 'http://localhost:5223/api';

getSubCategories() {
  return this.http.get<any[]>(`${this.baseUrl}/SubCategory`);
}

getAuthors() {
  return this.http.get<any[]>(`${this.baseUrl}/author`);
}

getCategories() {
  return this.http.get<any[]>(`${this.baseUrl}/Category`);
}
}
