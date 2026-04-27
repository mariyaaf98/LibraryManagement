using LibraryManagement.Domain.LoanEntity;
using LibraryManagement.Domain.ReservationEntity;
using LibraryManagement.Domain.Enums;
namespace LibraryManagement.Domain.UserEntity;

public class User : BaseEntity
{

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "MEMBER";

    public UserStatus Status { get; set; } = UserStatus.Active;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? ExternalId { get; set; }

    public decimal FinesOutstanding { get; set; }

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public ICollection<Reservation>? Reservations { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}