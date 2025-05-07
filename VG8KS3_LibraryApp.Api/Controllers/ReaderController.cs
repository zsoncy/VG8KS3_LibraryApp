using Microsoft.AspNetCore.Mvc;
using VG8KS3_LibraryApp.Api.Models;
using VG8KS3_LibraryApp.Api.Services;

namespace VG8KS3_LibraryApp.Api.Controllers;

[ApiController]
[Route("reader")]
public class ReaderController: ControllerBase
{
    private readonly IReaderService _readerService;

    public ReaderController(IReaderService readerService)
    {
        _readerService = readerService;
    }

    [HttpPost]
    public IActionResult Add([FromBody] Reader reader)
    {
        var existingReader = _readerService.Get(reader.ReaderId);

        if (existingReader is not null)
        {
            return Conflict();
        }
        
        _readerService.Add(reader);
        return Ok();
    }

    [HttpDelete("{readerId}")]
    public IActionResult Delete(int readerId)
    {
        var existingReader = _readerService.Get(readerId);

        if (existingReader is null)
        {
            return NotFound();
        }
        
        _readerService.Delete(readerId);
        return Ok();
    }

    [HttpGet]
    public ActionResult<List<Reader>> GetAll()
    {
        var readers = _readerService.Get();
        return Ok(readers);
    }

    [HttpGet("{readerId}")]
    public ActionResult<Reader> Get(int readerId)
    {
        var reader = _readerService.Get(readerId);

        if (reader is null)
        {
            return NotFound();
        }   
        
        return Ok(reader);
    }

    [HttpPut("{readerId}")]
    public IActionResult Update(int readerId, [FromBody] Reader reader)
    {
        if (readerId != reader.ReaderId)
        {
            return BadRequest();
        }
        
        var oldReader = _readerService.Get(readerId);

        if (oldReader is null)
        {
            return NotFound();
        }
        
        _readerService.Update(reader);
        return Ok();
    }
    
    
}