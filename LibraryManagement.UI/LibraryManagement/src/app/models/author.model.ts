// models/author.model.ts

export interface Author {
  id?:          string;
  givenName?:   string;
  familyName?:  string;
  fullName:     string;
  birthDate?:   string;
  deathDate?:   string;
  biography?:   string;
  createdAt?:   string;
  updatedAt?:   string;
  isDeleted?:   boolean;
}