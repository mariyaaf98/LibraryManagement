using LibraryManagement.Domain.CopyEntity;
using LibraryManagement.Application.CopyInterface;
using LibraryManagement.API.DTOs.Copy;
using LibraryManagement.Application.Exceptions;

public class CopyService
{
    private readonly ICopyRepository _repo;

    public CopyService(ICopyRepository repo)
    {
        _repo = repo;
    }

    // GET ALL
    public async Task<List<CopyResponseDto>> GetAllAsync()
    {
        var copies = await _repo.GetAllAsync();

        return copies.Select(c => new CopyResponseDto
        {
            Id = c.Id,
            BookId = c.BookId,
            Barcode = c.Barcode,
            AcquisitionDate = c.AcquisitionDate,
            Location = c.Location,
            Status = c.Status,
            Condition = c.Condition,
            Notes = c.Notes
        }).ToList();
    }

    // GET BY ID
    public async Task<CopyResponseDto> GetByIdAsync(Guid id)
    {
        var c = await _repo.GetByIdAsync(id);

        if (c == null)
            throw new NotFoundException("Copy not found");

        return new CopyResponseDto
        {
            Id = c.Id,
            BookId = c.BookId,
            Barcode = c.Barcode,
            AcquisitionDate = c.AcquisitionDate,
            Location = c.Location,
            Status = c.Status,
            Condition = c.Condition,
            Notes = c.Notes
        };
    }

    // CREATE
    public async Task<CopyResponseDto> CreateAsync(CreateCopyDto dto)
    {
        if (dto.BookId == Guid.Empty)
            throw new ArgumentException("BookId is required");

        var copy = new Copy
        {
            BookId = dto.BookId,
            Barcode = dto.Barcode,
            AcquisitionDate = dto.AcquisitionDate?.ToUniversalTime(),
            Location = dto.Location,
            Status = "AVAILABLE",
            Condition = "NEW"
        };

        await _repo.AddAsync(copy);
        await _repo.SaveChangesAsync();

        return new CopyResponseDto
        {
            Id = copy.Id,
            BookId = copy.BookId,
            Barcode = copy.Barcode,
            AcquisitionDate = copy.AcquisitionDate,
            Location = copy.Location,
            Status = copy.Status,
            Condition = copy.Condition,
            Notes = copy.Notes
        };
    }

    // UPDATE
    public async Task UpdateAsync(Guid id, UpdateCopyDto dto)
    {
        var copy = await _repo.GetByIdAsync(id);

        if (copy == null)
            throw new NotFoundException("Copy not found");

        copy.Status = dto.Status;
        copy.Condition = dto.Condition;
        copy.Location = dto.Location;
        copy.Notes = dto.Notes;

        _repo.Update(copy);
        await _repo.SaveChangesAsync();
    }

    // DELETE
    public async Task DeleteAsync(Guid id)
    {
        var copy = await _repo.GetByIdAsync(id);

        if (copy == null)
            throw new NotFoundException("Copy not found");

        _repo.Delete(copy);
        await _repo.SaveChangesAsync();
    }
}