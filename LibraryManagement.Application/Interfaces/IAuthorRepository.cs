using LibraryManagement.Domain.AuthorEntity;

namespace LibraryManagement.Application.AuthorRepository;

public interface IAuthorRepository
{
    Task<List<Author>> GetAllActiveAsync();
    Task<Author?> GetByIdAsync(Guid id);
    Task<Author> AddAsync(Author author);
    Task UpdateAsync(Author author);
}