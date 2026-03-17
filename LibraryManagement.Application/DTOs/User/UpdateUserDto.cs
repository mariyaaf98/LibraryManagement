using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.User;

public class UpdateUserDto
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? ExternalId { get; set; }

    public UserStatus Status { get; set; } 
}