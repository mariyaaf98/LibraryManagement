
namespace LibraryManagement.API.DTOs.Copy;
public class CopyResponseDto
{
    public Guid Id { get; set; }

    public Guid BookId { get; set; }

    public string Barcode { get; set; } = string.Empty;

    public DateTime? AcquisitionDate { get; set; }

    public string? Location { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Condition { get; set; } = string.Empty;

    public string? Notes { get; set; }
}