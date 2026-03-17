using LibraryManagement.Application.CategoryInterface;
using LibraryManagement.Domain.CategoryEntity;

namespace LibraryManagement.Application.CategoryService;

public class CategoryService : ICategoryRepository
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Category CreateCategory(Category category)
    {
        return _categoryRepository.CreateCategory(category);
    }

    public List<Category> GetCategories()
    {
        return _categoryRepository.GetCategories();
    }

    public Category? GetCategoryById(Guid id)
    {
        return _categoryRepository.GetCategoryById(id);
    }

    public void UpdateCategory(Category category)
    {
        _categoryRepository.UpdateCategory(category);
    }

    public bool DeleteCategory(Guid id)
    {
        return _categoryRepository.DeleteCategory(id);
    }
}