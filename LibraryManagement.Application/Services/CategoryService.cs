using LibraryManagement.Application.CategoryInterface;
using LibraryManagement.Domain.CategoryEntity;

namespace LibraryManagement.Application.CategoryService;

public class CategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _repo.GetAllAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _repo.GetByIdAsync(id);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        // 🔥 validation (important use of service)
        if (string.IsNullOrWhiteSpace(category.Name))
            throw new Exception("Category name is required");

        return await _repo.CreateAsync(category);
    }

    public async Task<bool> UpdateAsync(Category category)
    {
        return await _repo.UpdateAsync(category);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repo.DeleteAsync(id);
    }
}