using Microsoft.EntityFrameworkCore;
using LibraryManagement.Domain.UserEntity;
using LibraryManagement.Domain.BookAuthorEntity;
using LibraryManagement.Domain.AuthorEntity;
using LibraryManagement.Domain.CopyEntity;
using LibraryManagement.Domain.ReservationEntity;
using LibraryManagement.Domain.LoanEntity;
using LibraryManagement.Domain.BookEntity;
using LibraryManagement.Domain.SubCategoryEntity;
using LibraryManagement.Domain.CategoryEntity;
using LibraryManagement.Domain.BookCategoryEntity;

namespace LibraryManagement.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<SubCategory> SubCategories { get; set; }

    public DbSet<Author> Authors { get; set; }

    public DbSet<Book> Books { get; set; }

    public DbSet<BookAuthor> BookAuthors { get; set; }

    public DbSet<BookCategory> BookCategories { get; set; }

    public DbSet<Copy> Copies { get; set; }

    public DbSet<Loan> Loans { get; set; }

    public DbSet<Reservation> Reservations { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
        .Property(u => u.Status)
        .HasConversion<string>();


        // Category → SubCategory (One-to-Many)
        modelBuilder.Entity<SubCategory>()
            .HasOne(s => s.Category)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(s => s.CategoryId);


        // SubCategory → Book (One-to-Many)
        modelBuilder.Entity<Book>()
            .HasOne(b => b.SubCategory)
            .WithMany(s => s.Books)
            .HasForeignKey(b => b.SubCategoryId);


        // Book → Copy (One-to-Many)
        modelBuilder.Entity<Copy>()
            .HasOne(c => c.Book)
            .WithMany(b => b.Copies)
            .HasForeignKey(c => c.BookId);


        // User → Loan (One-to-Many)
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.User)
            .WithMany(u => u.Loans)
            .HasForeignKey(l => l.UserId);


        // Copy → Loan (One-to-Many)
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Copy)
            .WithMany(c => c.Loans)
            .HasForeignKey(l => l.CopyId);


        // User → Reservation (One-to-Many)
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reservations)
            .HasForeignKey(r => r.UserId);


        // Book → Reservation (One-to-Many)
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Book)
            .WithMany(b => b.Reservations)
            .HasForeignKey(r => r.BookId);


        // Book ↔ Author (Many-to-Many)
        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId);

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ba => ba.AuthorId);


        // Book ↔ Category (Many-to-Many)
        modelBuilder.Entity<BookCategory>()
            .HasKey(bc => new { bc.BookId, bc.CategoryId });

        modelBuilder.Entity<BookCategory>()
            .HasOne(bc => bc.Book)
            .WithMany(b => b.BookCategories)
            .HasForeignKey(bc => bc.BookId);

        modelBuilder.Entity<BookCategory>()
            .HasOne(bc => bc.Category)
            .WithMany(c => c.BookCategories)
            .HasForeignKey(bc => bc.CategoryId);
    }
}