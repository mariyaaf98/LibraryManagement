namespace LibraryManagement.API.DTOs.SubCategory;
public class CreateSubCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string? Description { get; set; }
}