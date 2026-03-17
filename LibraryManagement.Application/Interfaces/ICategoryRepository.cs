using LibraryManagement.Domain.CategoryEntity;

namespace LibraryManagement.Application.CategoryInterface;
public interface ICategoryRepository
{
    Category CreateCategory(Category category);

    List<Category> GetCategories();

    Category? GetCategoryById(Guid id);

    void UpdateCategory(Category category);

    bool DeleteCategory(Guid id);
}