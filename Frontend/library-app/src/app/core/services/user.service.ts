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

  // GET ALL USERS
  getUsers(): Observable<UserResponseDto[]> {
    return this.http.get<UserResponseDto[]>(this.apiUrl);
  }

  // GET USER BY ID (IMPORTANT FOR EDIT)
  getUserById(id: string): Observable<UserResponseDto> {
    return this.http.get<UserResponseDto>(`${this.apiUrl}/${id}`);
  }

  // CREATE USER
  createUser(data: CreateUserDto): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }

  // UPDATE USER
  updateUser(id: string, data: UpdateUserDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, data);
  }

  // BLOCK / UNBLOCK
  toggleBlock(id: string): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${id}/toggle-block`, {});
  }
}