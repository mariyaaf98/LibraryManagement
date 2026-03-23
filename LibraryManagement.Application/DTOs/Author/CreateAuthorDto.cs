namespace LibraryManagement.API.DTOs.Author;

public class CreateAuthorDto
{
   
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Biography { get; set; }
}