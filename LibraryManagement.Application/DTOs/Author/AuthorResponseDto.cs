namespace LibraryManagement.API.DTOs.Author;

public class AuthorResponseDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
}