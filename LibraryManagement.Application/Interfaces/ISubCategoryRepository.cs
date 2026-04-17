using LibraryManagement.Domain.SubCategoryEntity;

public interface ISubCategoryRepository
{
    Task<List<SubCategory>> GetAllAsync();
    Task<SubCategory?> GetByIdAsync(Guid id);
    Task<SubCategory> CreateAsync(SubCategory subCategory);
    Task UpdateAsync(SubCategory subCategory);
    Task DeleteAsync(Guid id);
}