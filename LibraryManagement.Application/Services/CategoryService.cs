using LibraryManagement.Application.CategoryInterface;
using LibraryManagement.Domain.CategoryEntity;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.API.DTOs.Category;

namespace LibraryManagement.Application.CategoryService;

public class CategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo)
    {
        _repo = repo;
    }

    // GET ALL
    public async Task<List<Category>> GetAllAsync()
    {
        return await _repo.GetAllAsync();
    }

    // GET BY ID
    public async Task<Category> GetByIdAsync(Guid id)
    {
        var category = await _repo.GetByIdAsync(id);

        if (category == null)
            throw new NotFoundException("Category not found");

        return category;
    }

    // CREATE
    public async Task<Category> CreateAsync(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
            throw new ArgumentException("Category name is required");

        return await _repo.CreateAsync(category);
    }

    // UPDATE
    public async Task UpdateAsync(Guid id, UpdateCategoryDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);

        if (existing == null)
            throw new NotFoundException("Category not found");

        existing.Name = dto.Name;
        existing.Description = dto.Description;

        await _repo.UpdateAsync(existing);
    }

    // DELETE
    public async Task DeleteAsync(Guid id)
    {
        var existing = await _repo.GetByIdAsync(id);

        if (existing == null)
            throw new NotFoundException("Category not found");

        await _repo.DeleteAsync(id);
    }
}