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

    // GET ALL
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

    // GET BY ID
    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _context.Books
            .Where(b => !b.IsDeleted)
            .Include(b => b.SubCategory)
            .Include(b => b.Copies)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    // CREATE
    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    // UPDATE
    public async Task UpdateAsync(Book book)
    {
        await _context.SaveChangesAsync(); 
    }

    // DELETE (Soft Delete)
    public async Task DeleteAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
            return;

        book.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    // SEARCH
    public async Task<IEnumerable<Book>> SearchAsync(
        string? search,
        string? genre,
        string? status)
    {
        var query = _context.Books
            .Where(b => !b.IsDeleted)
            .Include(b => b.SubCategory)
            .Include(b => b.Copies)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsQueryable();//is used to build a query step by step. 
                          // It does not run immediately. 
                          // The query runs only at the end when we call ToListAsync().


        // SEARCH TEXT
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(b =>
                b.Title.Contains(search) ||
                b.Isbn!.Contains(search) ||
                b.BookAuthors.Any(a => a.Author!.FullName.Contains(search))
            );
        }

        // GENRE FILTER
        if (!string.IsNullOrWhiteSpace(genre))
        {
            query = query.Where(b =>
                b.SubCategory != null &&
                b.SubCategory.Name == genre
            );
        }

        // STATUS FILTER
        if (!string.IsNullOrWhiteSpace(status))
        {
            status = status.ToLower();

            if (status == "available")
                query = query.Where(b => b.Copies.Count(c => c.Status == "AVAILABLE") > 0);

            else if (status == "low")
                query = query.Where(b =>
                    b.Copies.Count(c => c.Status == "AVAILABLE") > 0 &&
                    b.Copies.Count(c => c.Status == "AVAILABLE") <= 3);

            else if (status == "out")
                query = query.Where(b =>
                    b.Copies.Count(c => c.Status == "AVAILABLE") == 0);
        }

        return await query.ToListAsync();
    }
}