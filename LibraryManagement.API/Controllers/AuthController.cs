// using LibraryManagement.API.DTOs.Login;
// using Microsoft.AspNetCore.Mvc;

// [ApiController]
// [Route("api/auth")]
// public class AuthController : ControllerBase
// {
//     private readonly AuthService _authService;

//     public AuthController(AuthService authService)
//     {
//         _authService = authService;
//     }

//     [HttpPost("login")]
//     public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
//     {
//         try
//         {
//             Console.WriteLine($"Login attempt: {request?.Email}");

//             if (request == null)
//                 return BadRequest("Invalid request");

//             var user = await _authService.LoginAsync(request);

//             if (user == null)
//                 return BadRequest("Invalid email or password");

//             return Ok(new
//             {
//                 message = "Login successful",
//                 role = user.Role
//             });
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine("ERROR: " + ex.Message);
//             return StatusCode(500, ex.Message);
//         }
//     }
// }




using LibraryManagement.API.DTOs.Login;
using Microsoft.AspNetCore.Mvc;

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

        var user = await _authService.LoginAsync(request);

        if (user == null)
            return BadRequest("Invalid email or password");

        return Ok(new
        {
            message = "Login successful",
            role = user.Role
        });
    }
}