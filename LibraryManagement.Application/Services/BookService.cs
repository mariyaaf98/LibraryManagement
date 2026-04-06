using LibraryManagement.API.DTOs.Book;
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
            Authors = b.BookAuthors.Select(a => a.Author!.FullName).ToList(),
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
                .Select(a => a.Author!.FullName)
                .ToList(),
            Categories = b.BookCategories
                .Select(c => c.Category!.Name)
                .ToList()
        };
    }
    public async Task AddAsync(CreateBookDto dto)
    {
        var book = new Book
        {
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
                AuthorId = aid
            }).ToList();

        // Add Categories
        book.BookCategories = dto.CategoryIds
            .Select(cid => new BookCategory
            {
                CategoryId = cid
            }).ToList();

        // ✅ DEFAULT COPY
        book.Copies = new List<Copy>
    {
        new Copy
        {
            
            Barcode = $"BC-{DateTime.UtcNow:yyyyMMddHHmmss}",
            AcquisitionDate = DateTime.UtcNow,
            Location = "Main Shelf",
            Status = "AVAILABLE",
            Condition = "NEW"
        }
    };


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
            .Select(aid => new BookAuthor { AuthorId = aid })
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
            Authors = b.BookAuthors.Select(a => a.Author!.FullName).ToList(),
            Categories = b.BookCategories.Select(c => c.Category!.Name).ToList()
        });
    }
}