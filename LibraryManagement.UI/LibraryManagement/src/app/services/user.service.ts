import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { User } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {

  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5223/api/user'; 

  getUsers() {
    return this.http.get<User[]>(this.baseUrl);
  }

  createUser(data: any) {
    return this.http.post(this.baseUrl, data);
  }

  updateUser(id: string, data: any) {
    return this.http.put(`${this.baseUrl}/${id}`, data);
  }

  deleteUser(id: string) {
  return this.http.delete(`${this.baseUrl}/${id}`, {
    responseType: 'text'
  });
}
}