
using LibraryManagement.Domain.SubCategoryEntity;
using LibraryManagement.Application.CategoryInterface;
using LibraryManagement.API.DTOs.SubCategory;

namespace LibraryManagement.Application.SubCategoryService;

public class SubCategoryService
{
    private readonly ISubCategoryRepository _repo;
    private readonly ICategoryRepository _categoryRepo;

    public SubCategoryService(
        ISubCategoryRepository repo,
        ICategoryRepository categoryRepo)
    {
        _repo = repo;
        _categoryRepo = categoryRepo;
    }

    // ── Get All ─────────────────────────────────
    public async Task<List<SubCategoryResponseDto>> GetAllAsync()
    {
        var data = await _repo.GetAllAsync();

        return data.Select(x => new SubCategoryResponseDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CategoryId = x.CategoryId,
            CategoryName = x.Category!.Name
        }).ToList();
    }

    // ── Get By Id ───────────────────────────────
    public async Task<SubCategoryResponseDto?> GetByIdAsync(Guid id)
    {
        var x = await _repo.GetByIdAsync(id);

        if (x == null) return null;

        return new SubCategoryResponseDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CategoryId = x.CategoryId,
            CategoryName = x.Category!.Name
        };
    }

    // ── Create ──────────────────────────────────
    public async Task<SubCategoryResponseDto> CreateAsync(CreateSubCategoryDto dto)
    {
        // 🔥 Validate Category exists
        var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);

        if (category == null)
            throw new InvalidOperationException("Category not found");

        var subCategory = new SubCategory
        {
            Name = dto.Name,
            Description = dto.Description,
            CategoryId = dto.CategoryId
        };

        var created = await _repo.CreateAsync(subCategory);

        return new SubCategoryResponseDto
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description,
            CategoryId = created.CategoryId,
            CategoryName = category.Name
        };
    }

    // ── Update ──────────────────────────────────
    public async Task<bool> UpdateAsync(Guid id, UpdateSubCategoryDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);

        if (existing == null) return false;

        existing.Name = dto.Name;
        existing.Description = dto.Description;

        await _repo.UpdateAsync(existing);

        return true;
    }

    // ── Delete ──────────────────────────────────
    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repo.DeleteAsync(id);
    }
}