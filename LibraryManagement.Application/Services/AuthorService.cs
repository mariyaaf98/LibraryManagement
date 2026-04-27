using LibraryManagement.Domain.AuthorEntity;
using LibraryManagement.Application.AuthorRepository;
using LibraryManagement.API.DTOs.Author;
using LibraryManagement.API.DTOs.Book;
using LibraryManagement.Application.Exceptions;

namespace LibraryManagement.Application.AuthorService;

public class AuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
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


    public async Task<AuthorResponseDto> GetByIdAsync(Guid id)
    {
        var author = await _authorRepository.GetByIdAsync(id);

        if (author == null)
            throw new NotFoundException("Author not found");

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
            FullName = $"{dto.GivenName} {dto.FamilyName}",
            BirthDate = dto.BirthDate,
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


    public async Task UpdateAsync(Guid id, UpdateAuthorDto dto)
    {
        var author = await _authorRepository.GetByIdAsync(id);

        if (author == null)
            throw new NotFoundException("Author not found");

        author.GivenName = dto.GivenName;
        author.FamilyName = dto.FamilyName;
        author.FullName = $"{dto.GivenName} {dto.FamilyName}";
        author.BirthDate = dto.BirthDate;
        author.Biography = dto.Biography;
        author.UpdatedAt = DateTime.UtcNow;

        await _authorRepository.UpdateAsync(author);
    }


    public async Task DeleteAsync(Guid id)
    {
        var author = await _authorRepository.GetByIdAsync(id);

        if (author == null)
            throw new NotFoundException("Author not found");

        var books = await _authorRepository.GetBooksByAuthorAsync(id);

        if (books.Any())
            throw new BadRequestException("Cannot delete author. Author has associated books.");

        author.IsDeleted = true;
        author.UpdatedAt = DateTime.UtcNow;

        await _authorRepository.UpdateAsync(author);
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