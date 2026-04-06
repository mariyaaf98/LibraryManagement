using LibraryManagement.Application.CopyInterface;
using LibraryManagement.Domain.CopyEntity;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class CopyRepository : ICopyRepository
{
    private readonly AppDbContext _context;

    public CopyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Copy>> GetAllAsync()
    {
        return await _context.Copies.ToListAsync();
    }

    public async Task<Copy?> GetByIdAsync(Guid id)
    {
        return await _context.Copies.FindAsync(id);
    }

    public async Task AddAsync(Copy copy)
    {
        await _context.Copies.AddAsync(copy);
    }

    public void Update(Copy copy)
    {
        _context.Copies.Update(copy);
    }

    public void Delete(Copy copy)
    {
        _context.Copies.Remove(copy);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}