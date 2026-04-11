import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CategoryResponseDto } from '../models/category-response.dto';
import { CreateCategoryDto } from '../models/create-category.dto';
import { UpdateCategoryDto } from '../models/update-category.dto';
import { SubCategoryResponseDto } from '../models/subcategory-response.dto';
import { CreateSubCategoryDto } from '../models/create-subcategory.dto';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5223/api';

  // CATEGORY
  getCategories(): Observable<CategoryResponseDto[]> {
    return this.http.get<CategoryResponseDto[]>(`${this.baseUrl}/Category`);
  }

  createCategory(data: CreateCategoryDto): Observable<CategoryResponseDto> {
    return this.http.post<CategoryResponseDto>(`${this.baseUrl}/Category`, data);
  }

  updateCategory(id: string, data: UpdateCategoryDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/Category/${id}`, data);
  }

  deleteCategory(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/Category/${id}`);
  }

  // SUBCATEGORY
  getSubCategories(): Observable<SubCategoryResponseDto[]> {
    return this.http.get<SubCategoryResponseDto[]>(`${this.baseUrl}/SubCategory`);
  }

  createSubCategory(data: CreateSubCategoryDto): Observable<SubCategoryResponseDto> {
    return this.http.post<SubCategoryResponseDto>(`${this.baseUrl}/SubCategory`, data);
  }

  updateSubCategory(id: string, data: any): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/SubCategory/${id}`, data);
  }

  deleteSubCategory(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/SubCategory/${id}`);
  }
}