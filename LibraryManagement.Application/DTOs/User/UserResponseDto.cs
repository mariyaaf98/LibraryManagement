namespace LibraryManagement.Application.DTOs.User;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public decimal FinesOutstanding { get; set; }
   public string Status { get; set; } = string.Empty;
}