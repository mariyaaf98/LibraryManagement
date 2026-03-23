using Microsoft.AspNetCore.Mvc;
using LibraryManagement.API.DTOs.Author;
using LibraryManagement.Domain.AuthorEntity;
using LibraryManagement.Application.AuthorRepository;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/author")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorRepository _repository;

    public AuthorController(IAuthorRepository repository)
    {
        _repository = repository;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAuthors()
    {
        var authors = await _repository.GetAllActiveAsync();

        var result = authors.Select(a => new AuthorResponseDto
        {
            Id = a.Id,
            FullName = a.FullName,
            BirthDate = a.BirthDate
        });

        return Ok(result);
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthorById(Guid id)
    {
        var author = await _repository.GetByIdAsync(id);

        if (author == null)
            return NotFound("Author not found");

        var result = new AuthorResponseDto
        {
            Id = author.Id,
            FullName = author.FullName,
            BirthDate = author.BirthDate
        };

        return Ok(result);
    }

    
    [HttpPost]
    public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto dto)
    {
        var author = new Author
        {
            GivenName = dto.GivenName,
            FamilyName = dto.FamilyName,
            FullName = $"{dto.GivenName} {dto.FamilyName}",
            BirthDate = dto.BirthDate,
            Biography = dto.Biography,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(author);

        var result = new AuthorResponseDto
        {
            Id = author.Id,
            FullName = author.FullName,
            BirthDate = author.BirthDate
        };

        return Ok(result);
    }

   
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] UpdateAuthorDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch");

        var author = await _repository.GetByIdAsync(id);

        if (author == null)
            return NotFound("Author not found");

        author.GivenName = dto.GivenName;
        author.FamilyName = dto.FamilyName;
        author.FullName = $"{dto.GivenName ?? ""} {dto.FamilyName ?? ""}".Trim();

        author.BirthDate = dto.BirthDate.HasValue
            ? DateTime.SpecifyKind(dto.BirthDate.Value, DateTimeKind.Utc)
            : null;

        author.Biography = dto.Biography;
        author.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(author);

        return NoContent();
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(Guid id)
    {
        var author = await _repository.GetByIdAsync(id);

        if (author == null)
            return NotFound("Author not found");

        author.IsDeleted = true;
        author.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(author);

        return NoContent();
    }
}