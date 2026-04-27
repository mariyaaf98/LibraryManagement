import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

// Request
export interface LoginRequest {
  email: string;
  password: string;
}

// Responses
export interface LoginResponse {
  message: string;
  role: string;
}

export interface RefreshResponse {
  message: string;
}

export interface LogoutResponse {
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = 'http://localhost:5223/api/auth';

  constructor(private http: HttpClient) {}

  // LOGIN
  login(data: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.baseUrl}/login`,
      data
    );
  }

  // REFRESH TOKEN
  refreshToken(): Observable<RefreshResponse> {
    return this.http.post<RefreshResponse>(
      `${this.baseUrl}/refresh`,
      {}
    );
  }

  // LOGOUT
  logout(): Observable<LogoutResponse> {
    return this.http.post<LogoutResponse>(
      `${this.baseUrl}/logout`,
      {}
    );
  }
}