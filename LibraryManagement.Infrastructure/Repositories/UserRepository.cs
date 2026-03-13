using LibraryManagement.Application.Interfaces;
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

    public List<User> GetUsers()
    {
        return _context.Users.Where(u => !u.IsDeleted).ToList();
    }

    public User? GetUserById(Guid id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
    }

    public User CreateUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public User UpdateUser(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
        return user;
    }
    
}