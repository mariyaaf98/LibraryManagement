namespace LibraryManagement.API.DTOs.Login;
public class LoginRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}