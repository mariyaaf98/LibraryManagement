export interface BookResponseDto {
  id: string;
  title: string;
  isbn?: string;
  publishedYear?: number;
  coverImageUrl?: string;
  subCategoryName?: string;

  authors: string[];
  categories: string[];

  totalCopies: number; 
}