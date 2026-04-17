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

    // GET ALL 
    public async Task<List<SubCategory>> GetAllAsync()
    {
        return await _context.SubCategories
            .Where(x => !x.IsDeleted)
            .Include(x => x.Category)
            .ToListAsync();
    }

    // GET BY ID (exclude deleted)
    public async Task<SubCategory?> GetByIdAsync(Guid id)
    {
        return await _context.SubCategories
            .Where(x => !x.IsDeleted)
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // CREATE
    public async Task<SubCategory> CreateAsync(SubCategory subCategory)
    {
        _context.SubCategories.Add(subCategory);
        await _context.SaveChangesAsync();
        return subCategory;
    }

    //UPDATE 
    public async Task UpdateAsync(SubCategory subCategory)
    {
        await _context.SaveChangesAsync();
    }

    // SOFT DELETE
    public async Task DeleteAsync(Guid id)
    {
        var subCategory = await _context.SubCategories.FindAsync(id);

        if (subCategory == null)
            return;

        subCategory.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
}