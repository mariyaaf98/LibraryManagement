export interface CreateUserDto {
  fullName: string;
  email: string;
  password: string;
  role: string;
  phone?: string;
  address?: string;
}