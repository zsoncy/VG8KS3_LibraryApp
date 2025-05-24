using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VG8KS3_LibraryApp.Api.DataBase;
using VG8KS3_LibraryApp.Api.Models;
using VG8KS3_LibraryApp.Api.Services;
using VG8KS3_LibraryApp.Shared.Models;

namespace VG8KS3_LibraryApp.Api.Controllers;

[ApiController]
[Route("book")]
public class BookController: ControllerBase
{
    private readonly DataContext _dataContext;

    public BookController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] BookDto book)
    {
        var existingBook = await _dataContext.Books.FindAsync(book.BookId);

        if (existingBook is not null)
        {
            return Conflict();
        }
        
        _dataContext.Books.Add(book);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{bookId}")]
    public async Task<IActionResult> Delete(int bookId)
    {
        var existingBook = await _dataContext.Books.FindAsync(bookId);

        if (existingBook is null)
        {
            return NotFound();
        }
        
        _dataContext.Books.Remove(existingBook);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetAll()
    {
        var books = await _dataContext.Books.ToListAsync();
        return Ok(books);
    }

    [HttpGet("{bookId}")]
    public async Task<ActionResult<BookDto>> Get(int bookId)
    {
        var book = await _dataContext.Books.FindAsync(bookId);
            
        if (book is null)
        {
            return NotFound();
        }
        
        return Ok(book);
    }

    [HttpPut("{bookId}")]
    public async Task<IActionResult> Update(int bookId, [FromBody] BookDto book)
    {
        if (bookId != book.BookId)
        {
            return BadRequest();
        }
        
        var oldBook = await _dataContext.Books.FindAsync(bookId);

        if (oldBook is null)
        {
            return NotFound();
        }
        
        _dataContext.Books.Update(oldBook);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
    
}