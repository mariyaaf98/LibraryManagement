using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.CategoryService;
using LibraryManagement.Domain.CategoryEntity;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public IActionResult CreateCategory(Category category)
    {
        var result = _categoryService.CreateCategory(category);
        return Ok(result);
    }

    [HttpGet]
    public IActionResult GetCategories()
    {
        return Ok(_categoryService.GetCategories());
    }

    [HttpGet("{id}")]
    public IActionResult GetCategoryById(string id)
    {
        if (!Guid.TryParse(id, out Guid categoryId))
        {
            return BadRequest("Invalid Id format");
        }

        var category = _categoryService.GetCategoryById(categoryId);

        if (category == null)
        {
            return NotFound("Category not found");
        }

        return Ok(category);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCategory(string id, Category category)
    {
        if (!Guid.TryParse(id, out Guid categoryId))
        {
            return BadRequest("Invalid Id format");
        }

        if (categoryId != category.Id)
        {
            return BadRequest("Category ID mismatch");
        }

        _categoryService.UpdateCategory(category);

        return Ok("Category updated successfully");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCategory(string id)
    {
        if (!Guid.TryParse(id, out Guid categoryId))
        {
            return BadRequest("Invalid Id format");
        }

        var deleted = _categoryService.DeleteCategory(categoryId);

        if (!deleted)
        {
            return NotFound("Category not found");
        }

        return Ok("Category deleted successfully");
    }
}