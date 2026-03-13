using LibraryManagement.Domain.BookEntity;
using LibraryManagement.Domain.LoanEntity;

namespace LibraryManagement.Domain.CopyEntity;

public class Copy
{
    public Guid Id { get; set; }

    public Guid BookId { get; set; }

    public string Barcode { get; set; } = string.Empty;

    public DateTime? AcquisitionDate { get; set; }

    public string? Location { get; set; }

    public string Status { get; set; } = "AVAILABLE";

    public string Condition { get; set; } = "NEW";

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Book? Book { get; set; }

    public ICollection<Loan>? Loans { get; set; }
}