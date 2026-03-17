using LibraryManagement.Domain.SubCategoryEntity;
using LibraryManagement.Domain.BookCategoryEntity;

namespace LibraryManagement.Domain.CategoryEntity;

public class Category : BaseEntity
{

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();

    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
}