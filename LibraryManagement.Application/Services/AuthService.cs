using LibraryManagement.API.DTOs.Login;
using LibraryManagement.Application.UserInterface;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Common;
public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;

    public AuthService(IUserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<string> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");

        if (user.Status == UserStatus.Blocked)
            throw new UnauthorizedAccessException("User is blocked");

        // Generate JWT token
        var token = _tokenService.CreateToken(user);

        return token;
    }
}