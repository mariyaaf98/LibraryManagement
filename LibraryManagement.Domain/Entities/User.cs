using LibraryManagement.Domain.LoanEntity;
using LibraryManagement.Domain.ReservationEntity;

namespace LibraryManagement.Domain.UserEntity;
public class User
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "MEMBER";

    public string Status { get; set; } = "ACTIVE";

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? ExternalId { get; set; }

    public decimal FinesOutstanding { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
     public ICollection<Reservation>? Reservations { get; set; }
}