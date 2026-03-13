using LibraryManagement.Domain.UserEntity;
using LibraryManagement.Application.Interfaces;

namespace LibraryManagement.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository repository)
    {
        _userRepository = repository;
    }

    public List<User> GetUsers()
    {
        return _userRepository.GetUsers();
    }

    public User? GetUserById(Guid id)
    {
        return _userRepository.GetUserById(id);
    }

    public User CreateUser(User user)
    {
        return _userRepository.CreateUser(user);
    }

    public User UpdateUser(User user)
    {
        
        user.UpdatedAt = DateTime.UtcNow;
        return _userRepository.UpdateUser(user);
    }

    public bool DeleteUser(Guid userId)
    {
        var user = _userRepository.GetUserById(userId);

        if (user == null || user.IsDeleted)
        {
            return false;
        }

        user.IsDeleted = true;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.UpdateUser(user);

        return true;
    }
}