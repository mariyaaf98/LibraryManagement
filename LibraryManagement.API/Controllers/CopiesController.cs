using LibraryManagement.API.DTOs.Copy;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class CopiesController : ControllerBase
{
    private readonly CopyService _service;

    public CopiesController(CopyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<CopyResponseDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CopyResponseDto>> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CopyResponseDto>> Create(CreateCopyDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateCopyDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}