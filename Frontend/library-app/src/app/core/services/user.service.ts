import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserResponseDto } from '../models/user-response.dto';
import { CreateUserDto } from '../models/user-create.dto';
import { UpdateUserDto } from '../models/user-update.dto';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserService {

  private apiUrl = 'http://localhost:5223/api/User';

  constructor(private http: HttpClient) {}

  getUsers(): Observable<UserResponseDto[]> {
    return this.http.get<UserResponseDto[]>(this.apiUrl);
  }

  createUser(data: CreateUserDto) {
    return this.http.post(this.apiUrl, data);
  }

  updateUser(id: string, data: UpdateUserDto) {
    return this.http.put(`${this.apiUrl}/${id}`, data);
  }

  toggleBlock(id: string) {
    return this.http.patch(`${this.apiUrl}/${id}/toggle-block`, {});
  }
}