using LibraryManagement.Domain.BookEntity;
using LibraryManagement.Domain.CategoryEntity;

namespace LibraryManagement.Domain.BookCategoryEntity;

public class BookCategory
{
    public Guid BookId { get; set; }

    public Guid CategoryId { get; set; }

    public Book? Book { get; set; }

    public Category? Category { get; set; }
}