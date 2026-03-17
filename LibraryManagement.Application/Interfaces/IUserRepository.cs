using LibraryManagement.Domain.UserEntity;

namespace LibraryManagement.Application.UserInterface;

public interface IUserRepository
{
    List<User> GetUsers();

    User? GetUserById(Guid id);

    User? GetByEmail(string email);

    User CreateUser(User user);

    User UpdateUser(User user);

    void DeleteUser(User user); 
}