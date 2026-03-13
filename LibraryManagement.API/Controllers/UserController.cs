using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Domain.UserEntity;
using LibraryManagement.Application.Services;

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

    // Get all users
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _userService.GetUsers();
        return Ok(users);
    }

    // Get user by Id
    [HttpGet("{id}")]
    public IActionResult GetUserById(string id)
    {

        if (!Guid.TryParse(id, out Guid userId))
        {
            return BadRequest("Invalid user ID format");
        }

        var user = _userService.GetUserById(userId);

        if (user == null)
        {
            return NotFound("User not found");
        }

        return Ok(user);
    }

    // Create new user
    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        var result = _userService.CreateUser(user);
        return Ok(result);
    }

    // Update existing user
    [HttpPut("{id}")]
    public IActionResult UpdateUser(string id, [FromBody] User user)
    {
        if (!Guid.TryParse(id, out Guid userId))
        {
            return BadRequest("Invalid user ID format");
        }

        if (userId != user.Id)
        {
            return BadRequest("User ID mismatch");
        }

        var updatedUser = _userService.UpdateUser(user);

        if (updatedUser == null)
        {
            return NotFound("User not found");
        }

        return Ok(updatedUser);
    }


    // Delete user
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(String id)
    {
        if(!Guid.TryParse(id, out Guid userId))
        {
            return BadRequest("Invalid user ID format");
        }
        
        var deleted = _userService.DeleteUser(userId);

        if (!deleted)
        {
            return NotFound("User not found");
        }

        return Ok("User deleted successfully");
    }
}