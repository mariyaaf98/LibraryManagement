using LibraryManagement.Domain.CopyEntity;

namespace LibraryManagement.Application.CopyInterface;
public interface ICopyRepository
{
    Task<List<Copy>> GetAllAsync();
    Task<Copy?> GetByIdAsync(Guid id);
    Task AddAsync(Copy copy);
    void Update(Copy copy);
    void Delete(Copy copy);
    Task SaveChangesAsync();
}