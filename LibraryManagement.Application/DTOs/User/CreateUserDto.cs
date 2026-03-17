using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.User;

public class CreateUserDto
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty; 

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? ExternalId { get; set; }
    
    public string Role { get; set; } = "MEMBER";

    public UserStatus Status { get; set; } = UserStatus.Active; 
}