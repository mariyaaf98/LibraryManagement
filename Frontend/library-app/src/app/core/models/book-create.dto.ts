export interface CreateBookDto {
  title: string;
  subtitle?: string;
  isbn?: string;
  edition?: string;
  publishedYear?: number | null;
  language?: string;
  summary?: string;
  coverImageUrl?: string;

  subCategoryId: string;
  authorIds: string[];
  categoryIds: string[];
}