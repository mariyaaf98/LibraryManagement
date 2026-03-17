// models/category.model.ts

export interface Category {
  id?:          string;
  name:         string;
  description?: string;
  createdAt?:   string;
  updatedAt?:   string;
  isDeleted?:   boolean;
}