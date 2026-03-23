namespace LibraryManagement.API.DTOs.Author;

public class UpdateAuthorDto
{
    public Guid Id { get; set; }

    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }

    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }

    public string? Biography { get; set; }
}