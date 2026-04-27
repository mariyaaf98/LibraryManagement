using LibraryManagement.API.DTOs.Login;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (request == null)
            return BadRequest("Invalid request");

        var (accessToken, refreshToken) = await _authService.LoginAsync(request);

        // Store ACCESS TOKEN in cookie
        Response.Cookies.Append("accessToken", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // change to true in production (HTTPS)
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddMinutes(1),//AddHours(1)
            Path = "/"
        });

        // Store REFRESH TOKEN in cookie
        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/"
        });

        // Extract role 
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(accessToken);

        var role = jwtToken.Claims
            .FirstOrDefault(c => c.Type.Contains("role"))?.Value;

        return Ok(new
        {
            message = "Login successful",
            role = role
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized();

        var user = await _authService.GetUserByRefreshTokenAsync(refreshToken);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return Unauthorized();

        var newAccessToken = _authService.GenerateAccessToken(user);
        var newRefreshToken = _authService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _authService.UpdateUserAsync(user);

        // Store new ACCESS TOKEN in cookie
        Response.Cookies.Append("accessToken", newAccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddHours(1),
            Path = "/"
        });

        // Store new REFRESH TOKEN in cookie
        Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/"
        });

        return Ok(new { message = "Token refreshed" });
    }
}