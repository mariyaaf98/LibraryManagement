using LibraryManagement.Domain.UserEntity;

namespace LibraryManagement.Application.Interfaces;

public interface IUserRepository
{
    List<User> GetUsers();

    User? GetUserById(Guid id);

    User CreateUser(User user);

    User UpdateUser(User user);

    // bool DeleteUser(Guid id);
}