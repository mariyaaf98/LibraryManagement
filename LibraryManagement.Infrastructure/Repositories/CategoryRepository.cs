using LibraryManagement.Application.CategoryInterface;
using LibraryManagement.Domain.CategoryEntity;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.CategoryRepository;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public Category CreateCategory(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();

        return category;
    }

    public List<Category> GetCategories()
    {
        return _context.Categories
            .Where(c => !c.IsDeleted)
            .Include(c => c.SubCategories)
            .ToList();
    }

    public Category? GetCategoryById(Guid id)
    {
        return _context.Categories
            .Where(c => !c.IsDeleted)
            .Include(c => c.SubCategories)
            .FirstOrDefault(c => c.Id == id);
    }

    public void UpdateCategory(Category category)
    {
        var existingCategory = _context.Categories
            .FirstOrDefault(c => c.Id == category.Id && !c.IsDeleted);

        if (existingCategory == null)
        {
            throw new Exception("Category not found");
        }

        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;
        existingCategory.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
    }

    public bool DeleteCategory(Guid id)
    {
        var category = _context.Categories
            .FirstOrDefault(c => c.Id == id && !c.IsDeleted);

        if (category == null)
        {
            return false;
        }

        category.IsDeleted = true;

        _context.SaveChanges();

        return true;
    }
}