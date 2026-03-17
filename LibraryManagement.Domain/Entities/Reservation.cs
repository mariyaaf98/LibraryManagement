using LibraryManagement.Domain.UserEntity;
using LibraryManagement.Domain.BookEntity;


namespace LibraryManagement.Domain.ReservationEntity;

public class Reservation : BaseEntity
{

    public Guid UserId { get; set; }

    public Guid BookId { get; set; }

    public int Position { get; set; }

    public string State { get; set; } = "ACTIVE";

    public DateTime? ExpiresAt { get; set; }

    public User? User { get; set; }

    public Book? Book { get; set; }
}