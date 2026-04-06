
using LibraryManagement.Application.DTOs.User;
using LibraryManagement.Application.UserService;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController(UserService service)
    {
        _service = service;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _service.GetByIdAsync(id);

        if (user == null)
            return NotFound("User not found");

        return Ok(user);
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        var created = await _service.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (updated == null)
            return NotFound("User not found");

        return Ok(updated);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound("User not found");

        return NoContent();
    }


    [HttpPatch("{id}/toggle-block")]
    public async Task<IActionResult> ToggleBlock(Guid id)
    {
        var result = await _service.ToggleBlockAsync(id);

        if (!result)
            return NotFound("User not found");

        return NoContent();
    }
}