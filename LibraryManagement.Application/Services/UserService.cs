using LibraryManagement.Domain.UserEntity;
using LibraryManagement.Application.UserInterface;
using LibraryManagement.Application.DTOs.User;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.UserService;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository repository)
    {
        _userRepository = repository;
    }

    // ── Get all ─────────────────────────────────
    public List<UserDto> GetUsers()
    {
        var users = _userRepository.GetUsers();
        return users.Select(u => MapToDto(u)).ToList();
    }

    // ── Get by Id ───────────────────────────────
    public UserDto? GetUserById(Guid id)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null) return null;

        return MapToDto(user);
    }

    // ── Create ──────────────────────────────────
    public UserDto CreateUser(CreateUserDto dto)
{
    var existingUser = _userRepository.GetByEmail(dto.Email);
    if (existingUser != null)
        throw new Exception("Email already exists");

    if (dto.Password.Length < 6)
        throw new Exception("Password must be at least 6 characters");

    var user = new User
    {
        FullName = dto.FullName,
        Email = dto.Email,
        Phone = dto.Phone,
        Address = dto.Address,
        ExternalId = dto.ExternalId,

        Role = dto.Role,

        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        Status = dto.Status,
        CreatedAt = DateTime.UtcNow
    };

    var createdUser = _userRepository.CreateUser(user);

    return MapToDto(createdUser);
}

    // ── Update ──────────────────────────────────
    public UserDto? UpdateUser(Guid id, UpdateUserDto dto)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null) return null;

        // Optional: prevent duplicate email
        var existingUser = _userRepository.GetByEmail(dto.Email);
        if (existingUser != null && existingUser.Id != id)
            throw new Exception("Email already exists");

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.Phone = dto.Phone;
        user.Address = dto.Address;
        user.ExternalId = dto.ExternalId;
        user.UpdatedAt = DateTime.UtcNow;
        user.Status = dto.Status;

        var updatedUser = _userRepository.UpdateUser(user);

        return MapToDto(updatedUser);
    }

    // ── Delete ──────────────────────────────────
    public bool DeleteUser(Guid userId)
    {
        var user = _userRepository.GetUserById(userId);

        if (user == null || user.IsDeleted)
            return false;

        user.IsDeleted = true;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.UpdateUser(user);

        return true;
    }

    // ── Mapping ─────────────────────────────────
    private UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            Status = user.Status.ToString(),
            FinesOutstanding = user.FinesOutstanding
        };
    }
}