
using LibraryManagement.Domain.BookEntity;
using LibraryManagement.Domain.AuthorEntity;

namespace LibraryManagement.Domain.BookAuthorEntity;

public class BookAuthor
{
    public Guid BookId { get; set; }=Guid.NewGuid();

    public Guid AuthorId { get; set; }

    public Book? Book { get; set; }

    public Author? Author { get; set; }
}