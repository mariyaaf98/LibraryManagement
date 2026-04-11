import { AuthorResponseDto } from "./author-response.dto";
import { CopyDto } from "./copy.dto";
import { SubCategoryResponseDto } from "./subcategory-response.dto";



export interface BookResponseDto {
  SubCategoryResponseDto: any;
  id: string;
  title: string;

  isbn?: string;
  publishedYear?: number;
  coverImageUrl?: string;

  subCategory: SubCategoryResponseDto;

  authors: AuthorResponseDto[]; 
  categories: string[];

  language?: string;
  summary?: string;

  copies: CopyDto[];

  totalCopies: number;

  availableCopies?: number;

  status:boolean;
}