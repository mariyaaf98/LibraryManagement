using LibraryManagement.API.DTOs.Author;
using LibraryManagement.API.DTOs.Book;
using LibraryManagement.API.DTOs.Copy;
using LibraryManagement.Application.BookRepository;
using LibraryManagement.Domain.BookAuthorEntity;
using LibraryManagement.Domain.BookCategoryEntity;
using LibraryManagement.Domain.BookEntity;
using LibraryManagement.Domain.CopyEntity;

public class BookService
{
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BookResponseDto>> GetAllAsync()
    {
        var books = await _repository.GetAllAsync();

        return books.Select(b => new BookResponseDto
        {
            Id = b.Id,
            Title = b.Title,
            Isbn = b.Isbn,
            PublishedYear = b.PublishedYear,

            CoverImageUrl = b.CoverImageUrl,

            SubCategoryName = b.SubCategory!.Name,
            Authors = b.BookAuthors
    .Select(a => new AuthorResponseDto
    {
        Id = a.Author!.Id,
        FullName = a.Author.FullName,
        BirthDate = a.Author.BirthDate
    })
    .ToList(),
            Categories = b.BookCategories.Select(c => c.Category!.Name).ToList()
        });
    }


    public async Task<BookResponseDto?> GetByIdAsync(Guid id)
    {
        var b = await _repository.GetByIdAsync(id);

        if (b == null) return null;

        return new BookResponseDto
        {
            Id = b.Id,
            Title = b.Title,
            Isbn = b.Isbn,
            PublishedYear = b.PublishedYear,
            CoverImageUrl = b.CoverImageUrl,

            SubCategoryName = b.SubCategory!.Name,

            Authors = b.BookAuthors
        .Select(a => new AuthorResponseDto
        {
            Id = a.Author!.Id,
            FullName = a.Author.FullName,
            BirthDate = a.Author.BirthDate
        })
        .ToList(),

            Categories = b.BookCategories
                .Select(c => c.Category!.Name)
                .ToList(),

            Language = b.Language,
            Summary = b.Summary,

            
            Copies = b.Copies.Select(c => new CopyResponseDto
            {
                Id = c.Id,
                BookId = c.BookId,
                Barcode = c.Barcode,
                AcquisitionDate = c.AcquisitionDate,
                Location = c.Location,
                Status = c.Status,
                Condition = c.Condition,
                Notes = c.Notes
            }).ToList(),

            TotalCopies = b.Copies.Count(),

            AvailableCopies = b.Copies.Count(c => c.Status == "AVAILABLE")
        };
    }


    public async Task AddAsync(CreateBookDto dto)
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Subtitle = dto.Subtitle,
            Isbn = dto.Isbn,
            Edition = dto.Edition,
            PublishedYear = dto.PublishedYear,
            Language = dto.Language,
            Summary = dto.Summary,
            CoverImageUrl = dto.CoverImageUrl,
            SubCategoryId = dto.SubCategoryId
        };

        // Add Authors
        book.BookAuthors = dto.AuthorIds
            .Select(aid => new BookAuthor
            {
                BookId = book.Id,
                AuthorId = aid
            }).ToList();

        // Add Categories
        book.BookCategories = dto.CategoryIds
            .Select(cid => new BookCategory
            {
                CategoryId = cid
            }).ToList();



        await _repository.AddAsync(book);
    }

    public async Task UpdateAsync(Guid id, UpdateBookDto dto)
    {
        var book = await _repository.GetByIdAsync(id);
        if (book == null) return;

        book.Title = dto.Title;
        book.Subtitle = dto.Subtitle;
        book.Isbn = dto.Isbn;
        book.Edition = dto.Edition;
        book.PublishedYear = dto.PublishedYear;
        book.Language = dto.Language;
        book.Summary = dto.Summary;
        book.CoverImageUrl = dto.CoverImageUrl;
        book.SubCategoryId = dto.SubCategoryId;

        // Clear old relations
        book.BookAuthors.Clear();
        book.BookCategories.Clear();

        // Re-add
        book.BookAuthors = dto.AuthorIds
            .Select(aid => new BookAuthor
            {
                BookId = book.Id,
                AuthorId = aid
            })
            .ToList();

        book.BookCategories = dto.CategoryIds
            .Select(cid => new BookCategory { CategoryId = cid })
            .ToList();

        await _repository.UpdateAsync(book);
    }


    public async Task DeleteAsync(Guid id)
    {
        var book = await _repository.GetByIdAsync(id);

        if (book == null) return;

        await _repository.DeleteAsync(id);
    }


    public async Task<IEnumerable<BookResponseDto>> SearchBooksAsync(
    string? search,
    string? genre,
    string? status)
    {
        var books = await _repository.SearchAsync(search, genre, status);

        return books.Select(b => new BookResponseDto
        {
            Id = b.Id,
            Title = b.Title,
            Isbn = b.Isbn,
            PublishedYear = b.PublishedYear,
            CoverImageUrl = b.CoverImageUrl,

            TotalCopies = b.Copies.Count(),

            SubCategoryName = b.SubCategory!.Name,
            Authors = b.BookAuthors
    .Select(a => new AuthorResponseDto
    {
        Id = a.Author!.Id,
        FullName = a.Author.FullName,
        BirthDate = a.Author.BirthDate
    })
    .ToList(),
            Categories = b.BookCategories.Select(c => c.Category!.Name).ToList()
        });
    }
}