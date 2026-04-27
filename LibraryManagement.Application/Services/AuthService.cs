using LibraryManagement.API.DTOs.Login;
using LibraryManagement.Application.UserInterface;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Common;
using LibraryManagement.Domain.UserEntity;
public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;

    public AuthService(IUserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }


    public async Task<(string accessToken, string refreshToken)> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");

        if (user.Status == UserStatus.Blocked)
            throw new UnauthorizedAccessException("User is blocked");

        var accessToken = _tokenService.CreateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userRepository.UpdateAsync(user);

        return (accessToken, refreshToken);
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
{
    return await _userRepository.GetByRefreshTokenAsync(refreshToken);
}

public string GenerateAccessToken(User user)
{
    return _tokenService.CreateToken(user);
}

public string GenerateRefreshToken()
{
    return _tokenService.GenerateRefreshToken();
}

public async Task UpdateUserAsync(User user)
{
    await _userRepository.UpdateAsync(user);
}
}