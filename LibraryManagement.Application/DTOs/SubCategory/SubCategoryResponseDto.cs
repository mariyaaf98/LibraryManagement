namespace LibraryManagement.API.DTOs.SubCategory;
public class SubCategoryResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; internal set; }
}