
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Domain.SubCategoryEntity;

namespace LibraryManagement.Infrastructure.SubCategoryRepository;
public class SubCategoryRepository : ISubCategoryRepository
{
    private readonly AppDbContext _context;

    public SubCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SubCategory>> GetAllAsync()
    {
        return await _context.SubCategories
            .Include(x => x.Category) 
            .ToListAsync();
    }

    public async Task<SubCategory?> GetByIdAsync(Guid id)
    {
        return await _context.SubCategories
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<SubCategory> CreateAsync(SubCategory subCategory)
    {
        _context.SubCategories.Add(subCategory);
        await _context.SaveChangesAsync();
        return subCategory;
    }

    public async Task<bool> UpdateAsync(SubCategory subCategory)
    {
        var existing = await _context.SubCategories.FindAsync(subCategory.Id);

        if (existing == null) return false;

        existing.Name = subCategory.Name;
        existing.Description = subCategory.Description;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var subCategory = await _context.SubCategories.FindAsync(id);

        if (subCategory == null) return false;

        _context.SubCategories.Remove(subCategory);
        await _context.SaveChangesAsync();
        return true;
    }
}