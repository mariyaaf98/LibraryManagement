using LibraryManagement.Domain.AuthorEntity;
using LibraryManagement.Domain.BookEntity;

namespace LibraryManagement.Application.AuthorRepository;

public interface IAuthorRepository
{
    Task<List<Author>> GetAllActiveAsync();
    Task<Author?> GetByIdAsync(Guid id);
    Task<Author> AddAsync(Author author);
    Task UpdateAsync(Author author);

    Task<IEnumerable<Book>> GetBooksByAuthorAsync(Guid authorId);
}