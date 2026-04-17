// using LibraryManagement.API.DTOs.Login;
// using LibraryManagement.Application.UserInterface;
// using LibraryManagement.Domain.UserEntity;



// public class AuthService
// {
//     private readonly IUserRepository _userRepository;

//     public AuthService(IUserRepository userRepository)
//     {
//         _userRepository = userRepository;
//     }

//     public async Task<User?> LoginAsync(LoginRequestDto request)
// {
//     var user = await _userRepository.GetByEmailAsync(request.Email);

//     if (user == null || string.IsNullOrEmpty(user.PasswordHash))
//         return null;

//     try
//     {
//         var isValid = BCrypt.Net.BCrypt.Verify(
//             request.Password, user.PasswordHash);

//         return isValid ? user : null;
//     }
//     catch
//     {
//         return null;
//     }
// }
// }


using LibraryManagement.API.DTOs.Login;
using LibraryManagement.Application.UserInterface;
using LibraryManagement.Domain.UserEntity;
using LibraryManagement.Domain.Enums;
public class AuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
            return null;

        Console.WriteLine($"User Status: {user.Status}");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        // IMPORTANT CHECK
        if (user.Status == UserStatus.Blocked)
        {
            
            throw new UnauthorizedAccessException("User is blocked");
        }
        return user;
    }
}