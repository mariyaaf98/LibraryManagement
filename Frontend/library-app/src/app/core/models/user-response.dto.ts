export interface UserResponseDto {
  id: string;
  fullName: string;
  email: string;
  role: string;
  phone?: string;
  address?: string;   
  finesOutstanding: number;
  status: string;
}