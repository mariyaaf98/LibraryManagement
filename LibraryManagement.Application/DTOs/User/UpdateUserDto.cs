using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.User;

public class UpdateUserDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "MEMBER";
    public string? Phone { get; set; }
    public string? Address { get; set; }
}