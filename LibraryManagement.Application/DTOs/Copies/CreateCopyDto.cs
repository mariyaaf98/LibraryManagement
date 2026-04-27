namespace LibraryManagement.API.DTOs.Copy;
public class CreateCopyDto
{
    public Guid BookId { get; set; }
    public DateTime? AcquisitionDate { get; set; }
    public string? Location { get; set; }
}