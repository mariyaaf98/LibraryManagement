using Microsoft.EntityFrameworkCore;
using LibraryManagement.Domain.BookEntity;
using LibraryManagement.Application.BookRepository;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.BookRepository;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books
        .Where(b => !b.IsDeleted)
        .Include(b => b.SubCategory)
        .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
        .Include(b => b.BookCategories)
            .ThenInclude(bc => bc.Category)
        .ToListAsync();
    }


    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _context.Books
            .Include(b => b.SubCategory)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
    }

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null) return;

        book.IsDeleted = true;

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Book>> SearchAsync(string? search, string? genre, string? status)
    {
        var query = _context.Books
            .Where(b => !b.IsDeleted)
            .Include(b => b.SubCategory)
            .Include(b => b.Copies)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsQueryable();

        // 🔍 Search
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(b =>
                b.Title.Contains(search) ||
                b.Isbn!.Contains(search) ||
                b.BookAuthors.Any(a => a.Author!.FullName.Contains(search))
            );
        }

        // catedory
        if (!string.IsNullOrWhiteSpace(genre))
        {
            query = query.Where(b => b.SubCategory!.Name == genre);
        }

        //  Status
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (status == "available")
                query = query.Where(b => b.Copies.Count > 3);

            else if (status == "low")
                query = query.Where(b => b.Copies.Count > 0 && b.Copies.Count <= 3);

            else if (status == "out")
                query = query.Where(b => b.Copies.Count == 0);
        }

        return await query.ToListAsync();
    }
}