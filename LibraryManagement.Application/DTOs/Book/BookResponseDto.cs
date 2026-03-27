namespace LibraryManagement.API.DTOs.Book;
public class BookResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Isbn { get; set; }
    public int? PublishedYear { get; set; }

    public string? CoverImageUrl { get; set; }

    public string? SubCategoryName { get; set; }

    public List<string> Authors { get; set; } = new();
    public List<string> Categories { get; set; } = new();

    public int TotalCopies { get; set; }
}