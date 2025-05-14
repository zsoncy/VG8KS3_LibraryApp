using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VG8KS3_LibraryApp.Api.DataBase;
using VG8KS3_LibraryApp.Api.Models;
using VG8KS3_LibraryApp.Api.Services;

namespace VG8KS3_LibraryApp.Api.Controllers;

[ApiController]
[Route("reader")]
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
    public async Task<ActionResult<List<Reader>>> GetAll()
    {
        var readers = await _dataContext.Readers.ToListAsync();
        return Ok(readers);
    }

    [HttpGet("{readerId}")]
    public async Task<ActionResult<Reader>> Get(int readerId)
    {
        var reader = await _dataContext.Readers.FindAsync(readerId);

        if (reader is null)
        {
            return NotFound();
        }   
        
        return Ok(reader);
    }

    [HttpPut("{readerId}")]
    public async Task<IActionResult> Update(int readerId, [FromBody] Reader reader)
    {
        if (readerId != reader.ReaderId)
        {
            return BadRequest();
        }
        
        var oldReader = await _dataContext.Readers.FindAsync(readerId);

        if (oldReader is null)
        {
            return NotFound();
        }
        
        _dataContext.Readers.Update(reader);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
    
    
}