using LibraryManagement.Domain.UserEntity;
using LibraryManagement.Application.UserInterface;
using LibraryManagement.Application.DTOs.User;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.UserService;


public class UserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    
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

   
    public async Task<UserResponseDto?> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return null;

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

    
    public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
    {
        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role, // direct (no auth check now)
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

    
    public async Task<UserResponseDto?> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return null;

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

    
    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }


    public async Task<bool> ToggleBlockAsync(Guid id)
{
    var user = await _repository.GetByIdAsync(id);
    if (user == null) return false;

    user.Status = user.Status == UserStatus.Blocked
        ? UserStatus.Active
        : UserStatus.Blocked;

    await _repository.UpdateAsync(user);

    return true;
}
}