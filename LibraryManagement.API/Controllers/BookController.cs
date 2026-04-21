using Microsoft.AspNetCore.Mvc;
using LibraryManagement.API.DTOs.Book;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookController : ControllerBase
{
    private readonly BookService _service;

    public BookController(BookService service)
    {
        _service = service;
    }

  
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _service.GetAllAsync();
        return Ok(books);
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var book = await _service.GetByIdAsync(id);
        return Ok(book);
    }

   
    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Create([FromBody] CreateBookDto dto)
    {
        await _service.AddAsync(dto);

        return Ok(new
        {
            message = "Book created successfully"
        });
    }

    
    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBookDto dto)
    {
        
        await _service.UpdateAsync(id, dto);

        return Ok(new
        {
            message = "Book updated successfully"
        });
    }

    
    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    //SEARCH
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? search = null,
        [FromQuery] string? genre = null,
        [FromQuery] string? status = null)
    {
        var result = await _service.SearchBooksAsync(search, genre, status);
        return Ok(result);
    }
}

//[FromBody]-Get data from HTTP request BODY
//[FromQuery]-Get data from URL query string