namespace LibraryManagement.API.DTOs.Book;
public class CreateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string? Isbn { get; set; }
    public string? Edition { get; set; }
    public int? PublishedYear { get; set; }
    public string? Language { get; set; }
    public string? Summary { get; set; }
    public string? CoverImageUrl { get; set; }

    public Guid SubCategoryId { get; set; }

    public List<Guid> AuthorIds { get; set; } = new();
    public List<Guid> CategoryIds { get; set; } = new();
}