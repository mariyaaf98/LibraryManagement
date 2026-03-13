using LibraryManagement.Domain.BookAuthorEntity;
namespace LibraryManagement.Domain.AuthorEntity;
public class Author
{
    public Guid Id { get; set; }

    public string? GivenName { get; set; }

    public string? FamilyName { get; set; }

    public string FullName { get; set; } = string.Empty;

    public DateTime? BirthDate { get; set; }

    public DateTime? DeathDate { get; set; }

    public string? Biography { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<BookAuthor>? BookAuthors { get; set; }
}