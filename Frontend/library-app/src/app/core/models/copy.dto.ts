export interface CopyDto {
  id: string;
  bookId: string;

  barcode: string;

  acquisitionDate?: string;
  location?: string;

  status: string;
  condition: string;

  notes?: string;
}