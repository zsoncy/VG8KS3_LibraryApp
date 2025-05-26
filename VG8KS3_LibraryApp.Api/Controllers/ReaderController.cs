using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VG8KS3_LibraryApp.Api.DataBase;
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
    public async Task<IActionResult> Add([FromBody] Reader reader)
    {
        var existingReader = await _dataContext.Readers.FindAsync(reader.ReaderId);

        if (existingReader is not null)
        {
            return Conflict();
        }
        
        var newReader = new Reader()
        {
            Name = reader.Name,
            Adress = reader.Adress, 
            DateOfBirth = reader.DateOfBirth,
        };
        
        _dataContext.Readers.Add(newReader);
        await _dataContext.SaveChangesAsync();
        return Ok(newReader);
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
    public async Task<ActionResult<List<Reader>>> GetAll()
    {
        var readers = await _dataContext.Readers.ToListAsync();
        return Ok(readers);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Reader>>> SearchReadersByName([FromQuery] string name)
    {
        var matchedReaders = await _dataContext.Readers
            .Where(r => r.Name.ToLower().Contains(name.ToLower()))
            .Select(r => new Reader()
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
    public async Task<ActionResult<Reader>> Get(int readerId)
    {
        var reader = await _dataContext.Readers.FindAsync(readerId);

        if (reader is null)
        {
            return NotFound();
        }   
        
        var newReader = new Reader()
        {
            ReaderId = reader.ReaderId,
            Name = reader.Name,
            Adress = reader.Adress,
            DateOfBirth = reader.DateOfBirth
        };

        return Ok(newReader);
    }

    [HttpPut("{readerId}")]
    public async Task<IActionResult> Update(int readerId, [FromBody] Reader reader)
    {
        if (readerId != reader.ReaderId)
        {
            return BadRequest("Mismatched reader ID.");
        }
        
        var existingReader = await _dataContext.Readers.FindAsync(readerId);

        if (existingReader is null)
        {
            return NotFound();
        }
        
        existingReader.Name = reader.Name;
        existingReader.Adress = reader.Adress;
        existingReader.DateOfBirth = reader.DateOfBirth;
        
        await _dataContext.SaveChangesAsync();
        return NoContent();
        
    }
    
    
}