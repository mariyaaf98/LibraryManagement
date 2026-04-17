using LibraryManagement.Domain.SubCategoryEntity;
using LibraryManagement.Application.CategoryInterface;
using LibraryManagement.API.DTOs.SubCategory;
using LibraryManagement.Application.Exceptions;

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

    // GET ALL
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

    // GET BY ID
    public async Task<SubCategoryResponseDto> GetByIdAsync(Guid id)
    {
        var x = await _repo.GetByIdAsync(id);

        if (x == null)
            throw new NotFoundException("SubCategory not found");

        return new SubCategoryResponseDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CategoryId = x.CategoryId,
            CategoryName = x.Category!.Name
        };
    }

    // CREATE
    public async Task<SubCategoryResponseDto> CreateAsync(CreateSubCategoryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Name is required");

        var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);

        if (category == null)
            throw new NotFoundException("Category not found");

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

    // UPDATE
    public async Task UpdateAsync(Guid id, UpdateSubCategoryDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);

        if (existing == null)
            throw new NotFoundException("SubCategory not found");

        existing.Name = dto.Name;
        existing.Description = dto.Description;

        await _repo.UpdateAsync(existing);
    }

    // DELETE
    public async Task DeleteAsync(Guid id)
    {
        var existing = await _repo.GetByIdAsync(id);

        if (existing == null)
            throw new NotFoundException("SubCategory not found");

        await _repo.DeleteAsync(id);
    }
}