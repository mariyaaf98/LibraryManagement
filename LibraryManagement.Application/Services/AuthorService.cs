using LibraryManagement.Domain.AuthorEntity;
using LibraryManagement.Application.AuthorRepository;
using LibraryManagement.API.DTOs.Author;
using LibraryManagement.API.DTOs.Book;

namespace LibraryManagement.Application.AuthorService;

public class AuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository repository)
    {
        _authorRepository = repository;
    }


    public async Task<List<AuthorResponseDto>> GetAllAsync()
    {
        var authors = await _authorRepository.GetAllActiveAsync();

        return authors.Select(a => new AuthorResponseDto
        {
            Id = a.Id,
            FullName = a.FullName,
            BirthDate = a.BirthDate
        }).ToList();
    }


    public async Task<AuthorResponseDto?> GetByIdAsync(Guid id)
    {
        var author = await _authorRepository.GetByIdAsync(id);

        if (author == null)
            return null;

        return new AuthorResponseDto
        {
            Id = author.Id,
            FullName = author.FullName,
            BirthDate = author.BirthDate
        };
    }


    public async Task<AuthorResponseDto> CreateAsync(CreateAuthorDto dto)
    {
        var author = new Author
        {
            GivenName = dto.GivenName,
            FamilyName = dto.FamilyName,
            FullName = $"{dto.GivenName ?? ""} {dto.FamilyName ?? ""}".Trim(),

            BirthDate = dto.BirthDate.HasValue
                ? DateTime.SpecifyKind(dto.BirthDate.Value, DateTimeKind.Utc)
                : null,

            Biography = dto.Biography,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _authorRepository.AddAsync(author);

        return new AuthorResponseDto
        {
            Id = author.Id,
            FullName = author.FullName,
            BirthDate = author.BirthDate
        };
    }


    public async Task<bool> UpdateAsync(UpdateAuthorDto dto)
    {
        var existingAuthor = await _authorRepository.GetByIdAsync(dto.Id);

        if (existingAuthor == null || existingAuthor.IsDeleted)
            return false;

        existingAuthor.GivenName = dto.GivenName;
        existingAuthor.FamilyName = dto.FamilyName;
        existingAuthor.FullName = $"{dto.GivenName ?? ""} {dto.FamilyName ?? ""}".Trim();

        existingAuthor.BirthDate = dto.BirthDate.HasValue
            ? DateTime.SpecifyKind(dto.BirthDate.Value, DateTimeKind.Utc)
            : null;

        existingAuthor.Biography = dto.Biography;
        existingAuthor.UpdatedAt = DateTime.UtcNow;

        await _authorRepository.UpdateAsync(existingAuthor);

        return true;
    }

    // ✅ DELETE (Soft Delete)
    public async Task<bool> DeleteAsync(Guid id)
    {
        var author = await _authorRepository.GetByIdAsync(id);

        if (author == null || author.IsDeleted)
            return false;

        author.IsDeleted = true;
        author.UpdatedAt = DateTime.UtcNow;

        await _authorRepository.UpdateAsync(author);

        return true;
    }

     public async Task<IEnumerable<BookResponseDto>> GetBooksByAuthorAsync(Guid authorId)
    {
        var books = await _authorRepository.GetBooksByAuthorAsync(authorId);

        return books.Select(b => new BookResponseDto
        {
            Id = b.Id,
            Title = b.Title,
            CoverImageUrl = b.CoverImageUrl
        });
    }
}