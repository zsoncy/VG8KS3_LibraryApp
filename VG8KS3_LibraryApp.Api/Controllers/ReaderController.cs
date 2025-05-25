using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VG8KS3_LibraryApp.Api.DataBase;
using VG8KS3_LibraryApp.Api.Models;
using VG8KS3_LibraryApp.Api.Services;
using VG8KS3_LibraryApp.Shared.Models;

namespace VG8KS3_LibraryApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReaderController: ControllerBase
{
    private readonly DataContext _dataContext;

    public ReaderController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] ReaderDto readerDto)
    {
        var existingReader = await _dataContext.Readers.FindAsync(readerDto.ReaderId);

        if (existingReader is not null)
        {
            return Conflict();
        }
        
        var reader = new Reader()
        {
            Name = readerDto.Name,
            Adress = readerDto.Adress, 
            DateOfBirth = readerDto.DateOfBirth,
        };
        
        _dataContext.Readers.Add(reader);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{readerId}")]
    public async Task<IActionResult> Delete(int readerId)
    {
        var existingReader = await _dataContext.Readers.FindAsync(readerId);

        if (existingReader is null)
        {
            return NotFound();
        }
        
        _dataContext.Readers.Remove(existingReader);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<ReaderDto>>> GetAll()
    {
        var readers = await _dataContext.Readers.ToListAsync();
        return Ok(readers);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ReaderDto>>> SearchReadersByName([FromQuery] string name)
    {
        var matchedReaders = await _dataContext.Readers
            .Where(r => r.Name.ToLower().Contains(name.ToLower()))
            .Select(r => new ReaderDto()
            {
                ReaderId = r.ReaderId,
                Name = r.Name,
                Adress = r.Adress,
                DateOfBirth = r.DateOfBirth
            })
            .ToListAsync();

        return Ok(matchedReaders);
    }

    [HttpGet("{readerId}")]
    public async Task<ActionResult<ReaderDto>> Get(int readerId)
    {
        var reader = await _dataContext.Readers.FindAsync(readerId);

        if (reader is null)
        {
            return NotFound();
        }   
        
        var readerDto = new ReaderDto()
        {
            ReaderId = reader.ReaderId,
            Name = reader.Name,
            Adress = reader.Adress,
            DateOfBirth = reader.DateOfBirth
        };

        return Ok(readerDto);
    }

    [HttpPut("{readerId}")]
    public async Task<IActionResult> Update(int readerId, [FromBody] ReaderDto readerDto)
    {
        if (readerId != readerDto.ReaderId)
        {
            return BadRequest("Mismatched reader ID.");
        }
        
        var existingReader = await _dataContext.Readers.FindAsync(readerId);

        if (existingReader is null)
        {
            return NotFound();
        }
        
        existingReader.Name = readerDto.Name;
        existingReader.Adress = readerDto.Adress;
        existingReader.DateOfBirth = readerDto.DateOfBirth;
        
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
    
    
}