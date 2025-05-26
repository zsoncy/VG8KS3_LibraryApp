using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VG8KS3_LibraryApp.Api.DataBase;
using VG8KS3_LibraryApp.Api.Services;
using VG8KS3_LibraryApp.Shared.Models;

namespace VG8KS3_LibraryApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController: ControllerBase
{
    private readonly DataContext _dataContext;

    public BookController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Book book)
    {
        var existingBook = await _dataContext.Books.FindAsync(book.BookId);

        if (existingBook is not null)
        {
            return Conflict();
        }
        
        var newBook = new Book()
        {
            Title = book.Title,
            Author = book.Author,
            Publisher = book.Publisher,
            ReleaseDate = book.ReleaseDate
        };
        
        _dataContext.Books.Add(newBook);
        await _dataContext.SaveChangesAsync();
        return Ok(newBook);

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
    public async Task<ActionResult<List<Book>>> GetAll()
    {
        var books = await _dataContext.Books.ToListAsync();
        return Ok(books);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Book>>> SearchBooksByTitle([FromQuery] string title)
    {
        var matchedBooks = await _dataContext.Books
            .Where(b => b.Title.ToLower().Contains(title.ToLower()))
            .Select(b => new Book()
            {
                BookId = b.BookId,
                Title = b.Title,
                Author = b.Author,
                Publisher = b.Publisher,
                ReleaseDate = b.ReleaseDate
            })
            .ToListAsync();

        return Ok(matchedBooks);
    }
    
    [HttpGet("{bookId}")]
    public async Task<ActionResult<Book>> Get(int bookId)
    {
        var book = await _dataContext.Books.FindAsync(bookId);

        if (book is null)
        {
            return NotFound();
        }

        var newBook = new Book()
        {
            BookId = book.BookId,
            Title = book.Title,
            Author = book.Author,
            Publisher = book.Publisher,
            ReleaseDate = book.ReleaseDate
        };

        return Ok(newBook);
    }

    [HttpPut("{bookId}")]
    public async Task<IActionResult> Update(int bookId, [FromBody] Book book)
    {
        if (bookId != book.BookId)
        {
            return BadRequest("Mismatched book ID.");
        }
        
        var existingBook = await _dataContext.Books.FindAsync(bookId);

        if (existingBook is null)
        {
            return NotFound();
        }
        
        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.Publisher = book.Publisher;
        existingBook.ReleaseDate = book.ReleaseDate;
        
        await _dataContext.SaveChangesAsync(); 
        return NoContent();
    }
    
}