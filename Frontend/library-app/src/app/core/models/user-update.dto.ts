export interface UpdateUserDto {
  fullName: string;
  email: string;
  role: string;
  phone?: string;
  address?: string;
}