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

        
        var token = await _authService.LoginAsync(request);

        //Store cookie
        Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddHours(1),

            //Cookie available for all API routes
            Path = "/"
        });

      
      //used to create, read, and validate JWT tokens
        var handler = new JwtSecurityTokenHandler();

        //decode JWT string and read its data
        var jwtToken = handler.ReadJwtToken(token);

        var role = jwtToken.Claims
            .FirstOrDefault(c => c.Type.Contains("role"))?.Value;

        return Ok(new
        {
            message = "Login successful",
            role = role,
        });
    }
}