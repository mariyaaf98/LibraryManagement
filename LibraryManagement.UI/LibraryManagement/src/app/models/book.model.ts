// models/book.model.ts

export interface Book {
  id?:             string;
  title:           string;
  subtitle?:       string;
  isbn?:           string;
  edition?:        string;
  publishedYear?:  number;
  language?:       string;
  summary?:        string;
  coverImageUrl?:  string;
  subCategoryId:   string;

  // from navigation properties (comes from backend JOIN)
  subCategoryName?:  string;
  categoryName?:     string;
  authors?:          string[];   // list of author names

  createdAt?:  string;
  updatedAt?:  string;
  isDeleted?:  boolean;
}