using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Domain.AuthorEntity;
using LibraryManagement.Application.AuthorService;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/author")]
public class AuthorController : ControllerBase
{
    private readonly AuthorService _authorService;

    public AuthorController(AuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public IActionResult GetAuthors()
    {
        var authors = _authorService.GetAuthors();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public IActionResult GetAuthorById(string id)
    {
        if (!Guid.TryParse(id, out Guid authorId))
        {
            return BadRequest("Invalid author ID format");
        }

        var author = _authorService.GetAuthorById(authorId);

        if (author == null)
        {
            return NotFound("Author not found");
        }

        return Ok(author);
    }

    [HttpPost]
    public IActionResult CreateAuthor([FromBody] Author author)
    {
        var result = _authorService.CreateAuthor(author);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateAuthor(string id, [FromBody] Author author)
    {
        if (!Guid.TryParse(id, out Guid authorId))
        {
            return BadRequest("Invalid author ID format");
        }

        if (authorId != author.Id)
        {
            return BadRequest("Author ID mismatch");
        }

        var updatedAuthor = _authorService.UpdateAuthor(author);

        if (updatedAuthor == null)
        {
            return NotFound("Author not found");
        }

        return Ok(updatedAuthor);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAuthor(string id)
    {
        if (!Guid.TryParse(id, out Guid authorId))
        {
            return BadRequest("Invalid author ID format");
        }

        var deleted = _authorService.DeleteAuthor(authorId);

        if (!deleted)
        {
            return NotFound("Author not found");
        }

        return Ok("Author deleted successfully");
    }
}