using LibraryManagement.Domain.CopyEntity;
using LibraryManagement.Domain.BookAuthorEntity;
using LibraryManagement.Domain.BookCategoryEntity;
using LibraryManagement.Domain.ReservationEntity;
using LibraryManagement.Domain.SubCategoryEntity;

namespace LibraryManagement.Domain.BookEntity;

public class Book : BaseEntity
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

    public SubCategory? SubCategory { get; set; }


    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();

    public ICollection<Copy> Copies { get; set; } = new List<Copy>();

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}