using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.AuthorService;
using LibraryManagement.API.DTOs.Author;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthorController : ControllerBase
{
    private readonly AuthorService _service;

    public AuthorController(AuthorService service)
    {
        _service = service; 
    }

    // GET ALL AUTHORS
    [HttpGet]
    public async Task<IActionResult> GetAuthors()
    {
        var authors = await _service.GetAllAsync();
        return Ok(authors);
    }

    // GET AUTHOR BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthorById(Guid id)
    {
        var author = await _service.GetByIdAsync(id);
        return Ok(author);
    }

    // CREATE AUTHOR
    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }

    // UPDATE AUTHOR
    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] UpdateAuthorDto dto)
    {
        if (id != dto.Id)
            throw new ArgumentException("ID mismatch");

        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    // DELETE AUTHOR (Soft Delete)
    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteAuthor(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    // GET BOOKS BY AUTHOR
    [HttpGet("{authorId}/books")]
    public async Task<IActionResult> GetBooksByAuthor(Guid authorId)
    {
        var books = await _service.GetBooksByAuthorAsync(authorId);
        return Ok(books);
    }
}