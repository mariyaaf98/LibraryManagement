using LibraryManagement.Application.AuthorInterface;
using LibraryManagement.Domain.AuthorEntity;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly AppDbContext _context;

    public AuthorRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Author> GetAuthors()
    {
        return _context.Authors.Where(a => !a.IsDeleted).ToList();
    }

    public Author? GetAuthorById(Guid id)
    {
        return _context.Authors.FirstOrDefault(a => a.Id == id && !a.IsDeleted);
    }

    public Author CreateAuthor(Author author)
    {
        _context.Authors.Add(author);
        _context.SaveChanges();

        return author;
    }

    public Author UpdateAuthor(Author author)
    {
        _context.Authors.Update(author);
        _context.SaveChanges();

        return author;
    }
}