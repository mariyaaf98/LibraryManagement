using LibraryManagement.Domain.BookEntity;
using LibraryManagement.Domain.CategoryEntity;

namespace LibraryManagement.Domain.SubCategoryEntity;

public class SubCategory
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Category? Category { get; set; } 

    public ICollection<Book> Books { get; set; } = new List<Book>();
}