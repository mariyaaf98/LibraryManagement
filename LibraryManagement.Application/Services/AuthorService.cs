using LibraryManagement.Domain.AuthorEntity;
using LibraryManagement.Application.AuthorInterface;

namespace LibraryManagement.Application.AuthorService;

public class AuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository repository)
    {
        _authorRepository = repository;
    }

    public List<Author> GetAuthors()
    {
        return _authorRepository.GetAuthors();
    }

    public Author? GetAuthorById(Guid id)
    {
        return _authorRepository.GetAuthorById(id);
    }

    public Author CreateAuthor(Author author)
    {
        return _authorRepository.CreateAuthor(author);
    }

    public Author? UpdateAuthor(Author author)
    {
        var existingAuthor = _authorRepository.GetAuthorById(author.Id);

        if (existingAuthor == null || existingAuthor.IsDeleted)
        {
            return null;
        }

        author.CreatedAt = existingAuthor.CreatedAt;
        author.UpdatedAt = DateTime.UtcNow;

        return _authorRepository.UpdateAuthor(author);
    }

    public bool DeleteAuthor(Guid authorId)
    {
        var author = _authorRepository.GetAuthorById(authorId);

        if (author == null || author.IsDeleted)
        {
            return false;
        }

        author.IsDeleted = true;
        author.UpdatedAt = DateTime.UtcNow;

        _authorRepository.UpdateAuthor(author);

        return true;
    }
}