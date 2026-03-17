using LibraryManagement.Domain.AuthorEntity;

namespace LibraryManagement.Application.AuthorInterface;

public interface IAuthorRepository
{
    List<Author> GetAuthors();

    Author? GetAuthorById(Guid id);

    Author CreateAuthor(Author author);

    Author UpdateAuthor(Author author);
}