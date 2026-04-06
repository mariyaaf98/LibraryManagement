
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Copy } from '../models/copy-response.dto';
import { CreateCopy } from '../models/copy-create.dto';

@Injectable({
  providedIn: 'root',
})
export class CopyService {
  private baseUrl = 'http://localhost:5223/api/copies';

  constructor(private http: HttpClient) { }

  getAll(): Observable<Copy[]> {
    return this.http.get<Copy[]>(this.baseUrl);
  }

  getByBookId(bookId: string): Observable<Copy[]> {
    return this.http.get<Copy[]>(`${this.baseUrl}?bookId=${bookId}`);
  }

  create(copy: CreateCopy): Observable<Copy> {
    return this.http.post<Copy>(this.baseUrl, copy);
  }
  delete(id: string) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
  update(id: string, copy: CreateCopy) {
    return this.http.put(`${this.baseUrl}/${id}`, copy);
  }

}

