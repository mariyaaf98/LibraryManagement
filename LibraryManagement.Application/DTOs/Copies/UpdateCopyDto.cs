namespace LibraryManagement.API.DTOs.Copy;
public class UpdateCopyDto
{
    public Guid Id { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Condition { get; set; } = string.Empty;

    public string? Location { get; set; }

    public string? Notes { get; set; }
}