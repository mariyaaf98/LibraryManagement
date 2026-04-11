export interface AuthorUpdateDto {
  id: string;
  givenName?: string;
  familyName?: string;
  birthDate?: string;
  deathDate?: string;
  biography?: string;
}