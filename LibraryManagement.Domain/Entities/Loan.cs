using LibraryManagement.Domain.UserEntity;
using LibraryManagement.Domain.CopyEntity;

namespace LibraryManagement.Domain.LoanEntity;

public class Loan
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid CopyId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string State { get; set; } = "ACTIVE";

    public int RenewalsUsed { get; set; }

    public string? Notes { get; set; }

    public User? User { get; set; }

    public Copy? Copy { get; set; }
}