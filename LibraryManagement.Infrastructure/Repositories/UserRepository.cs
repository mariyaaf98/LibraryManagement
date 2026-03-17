using LibraryManagement.Application.UserInterface;
using LibraryManagement.Domain.UserEntity;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    // ── Get all users ───────────────────────────
    public List<User> GetUsers()
    {
        return _context.Users
            .Where(u => !u.IsDeleted)
            .ToList();
    }

    // ── Get by Id ───────────────────────────────
    public User? GetUserById(Guid id)
    {
        return _context.Users
            .FirstOrDefault(u => u.Id == id && !u.IsDeleted);
    }

    // ── Get by Email (NEW) ──────────────────────
    public User? GetByEmail(string email)
    {
        return _context.Users
            .FirstOrDefault(u => u.Email == email && !u.IsDeleted);
    }

    // ── Create ──────────────────────────────────
    public User CreateUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    // ── Update ──────────────────────────────────
    public User UpdateUser(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
        return user;
    }

    // ── Delete (Soft delete) ────────────────────
    public void DeleteUser(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
}