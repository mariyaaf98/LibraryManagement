using LibraryManagement.Domain.SubCategoryEntity;
public interface ISubCategoryRepository
{
    Task<List<SubCategory>> GetAllAsync();
    Task<SubCategory?> GetByIdAsync(Guid id);
    Task<SubCategory> CreateAsync(SubCategory subCategory);
    Task<bool> UpdateAsync(SubCategory subCategory);
    Task<bool> DeleteAsync(Guid id);
}