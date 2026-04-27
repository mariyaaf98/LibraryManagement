using LibraryManagement.Domain.UserEntity;

namespace LibraryManagement.Application.UserInterface;

using LibraryManagement.Domain.UserEntity;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User> AddAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<bool> DeleteAsync(Guid id);

    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByRefreshTokenAsync(string refreshToken);

}