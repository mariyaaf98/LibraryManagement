export interface UserResponseDto {
  id: string;
  fullName: string;
  email: string;
  role: string;
  phone?: string;
  finesOutstanding: number;
  status: string;
}