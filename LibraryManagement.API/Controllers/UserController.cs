using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.UserService;
using LibraryManagement.Application.DTOs.User;


namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // ── Get all users ───────────────────────────
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _userService.GetUsers();
        return Ok(users);
    }

    // ── Get by Id ───────────────────────────────
    [HttpGet("{id}")]
    public IActionResult GetUserById(string id)
    {
        if (!Guid.TryParse(id, out Guid userId))
            return BadRequest("Invalid user ID format");

        var user = _userService.GetUserById(userId);

        if (user == null)
            return NotFound("User not found");

        return Ok(user);
    }

    // ── Create ──────────────────────────────────
    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserDto dto)
    {
        // BASIC VALIDATION (Controller level)
        if (string.IsNullOrWhiteSpace(dto.FullName) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest("Name, Email and Password are required");
        }

        try
        {
            var result = _userService.CreateUser(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // ── Update ──────────────────────────────────
    [HttpPut("{id}")]
    public IActionResult UpdateUser(string id, [FromBody] UpdateUserDto dto)
    {
        if (!Guid.TryParse(id, out Guid userId))
            return BadRequest("Invalid user ID format");

        if (string.IsNullOrWhiteSpace(dto.FullName) ||
            string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest("Name and Email are required");
        }

        var updatedUser = _userService.UpdateUser(userId, dto);

        if (updatedUser == null)
            return NotFound("User not found");

        return Ok(updatedUser);
    }

    // ── Delete ──────────────────────────────────
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(string id)
    {
        if (!Guid.TryParse(id, out Guid userId))
            return BadRequest("Invalid user ID format");

        var deleted = _userService.DeleteUser(userId);

        if (!deleted)
            return NotFound("User not found");

        return Ok("User deleted successfully");
    }
}