// namespace LibraryManagement.API.DTOs.Book;
// public class BookResponseDto
// {
//     public Guid Id { get; set; }
//     public string Title { get; set; } = string.Empty;
//     public string? Isbn { get; set; }
//     public int? PublishedYear { get; set; }

//     public string? CoverImageUrl { get; set; }

//     public string? SubCategoryName { get; set; }

//     public List<string> Authors { get; set; } = new();
//     public List<string> Categories { get; set; } = new();

//     public int TotalCopies { get; set; }
// }



using LibraryManagement.API.DTOs.Author;
using LibraryManagement.API.DTOs.Copy;

namespace LibraryManagement.API.DTOs.Book;

public class BookResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public string? Isbn { get; set; }
    public int? PublishedYear { get; set; }

    public string? CoverImageUrl { get; set; }
    public string? SubCategoryName { get; set; }
    public List<AuthorResponseDto>? Authors { get; set; }
    public List<string> Categories { get; set; } = new();

    public string? Language { get; set; }
    public string? Summary { get; set; }


    public List<CopyResponseDto> Copies { get; set; } = new();

    public int TotalCopies { get; set; }

    public int AvailableCopies { get; set; }
}