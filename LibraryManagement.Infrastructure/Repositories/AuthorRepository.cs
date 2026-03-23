using Microsoft.EntityFrameworkCore;
using LibraryManagement.Domain.AuthorEntity;
using LibraryManagement.Application.AuthorRepository;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly AppDbContext _context;

    public AuthorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Author>> GetAllActiveAsync()
    {
        return await _context.Authors
            .Where(a => !a.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(Guid id)
    {
        return await _context.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
    }

    public async Task<Author> AddAsync(Author author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task UpdateAsync(Author author)
    {
        _context.Entry(author).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}