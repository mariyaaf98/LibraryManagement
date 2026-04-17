using LibraryManagement.Domain.UserEntity;
using LibraryManagement.Application.UserInterface;
using LibraryManagement.Application.DTOs.User;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Application.Exceptions;

namespace LibraryManagement.Application.UserService;

public class UserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    // GET ALL
    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var users = await _repository.GetAllAsync();

        return users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            Role = u.Role,
            Phone = u.Phone,
            FinesOutstanding = u.FinesOutstanding,
            Status = u.Status.ToString()
        });
    }

    // GET BY ID
    public async Task<UserResponseDto> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User not found");

        return new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            Phone = user.Phone,
            FinesOutstanding = user.FinesOutstanding,
            Status = user.Status.ToString()
        };
    }

    // CREATE
    public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Email is required");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role,
            Phone = dto.Phone,
            Address = dto.Address
        };

        var created = await _repository.AddAsync(user);

        return new UserResponseDto
        {
            Id = created.Id,
            FullName = created.FullName,
            Email = created.Email,
            Role = created.Role,
            Phone = created.Phone,
            FinesOutstanding = created.FinesOutstanding,
            Status = created.Status.ToString()
        };
    }

    // UPDATE
    public async Task<UserResponseDto> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User not found");

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.Role = dto.Role;
        user.Phone = dto.Phone;
        user.Address = dto.Address;

        var updated = await _repository.UpdateAsync(user);

        return new UserResponseDto
        {
            Id = updated.Id,
            FullName = updated.FullName,
            Email = updated.Email,
            Role = updated.Role,
            Phone = updated.Phone,
            FinesOutstanding = updated.FinesOutstanding,
            Status = updated.Status.ToString()
        };
    }

    // DELETE
    public async Task DeleteAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User not found");

        await _repository.DeleteAsync(id);
    }

    // TOGGLE BLOCK
    public async Task ToggleBlockAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User not found");

        user.Status = user.Status == UserStatus.Blocked
            ? UserStatus.Active
            : UserStatus.Blocked;

        await _repository.UpdateAsync(user);
    }

    // GET BY EMAIL
    public async Task<UserResponseDto> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        var user = await _repository.GetByEmailAsync(email);

        if (user == null)
            throw new NotFoundException("User not found");

        return new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            Phone = user.Phone,
            FinesOutstanding = user.FinesOutstanding,
            Status = user.Status.ToString()
        };
    }

    // CHANGE PASSWORD
    public async Task ChangePasswordAsync(ChangePasswordDto dto)
    {
        var user = await _repository.GetByEmailAsync(dto.Email);

        if (user == null)
            throw new NotFoundException("User not found");

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid current password");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        await _repository.UpdateAsync(user);
    }
}