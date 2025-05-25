using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VG8KS3_LibraryApp.Api.DataBase;
using VG8KS3_LibraryApp.Api.Models;
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
    public async Task<IActionResult> Add([FromBody] BookDto bookDto)
    {
        var existingBook = await _dataContext.Books.FindAsync(bookDto.BookId);

        if (existingBook is not null)
        {
            return Conflict();
        }
        
        var book = new Book
        {
            Title = bookDto.Title,
            Author = bookDto.Author,
            Publisher = bookDto.Publisher,
            ReleaseDate = bookDto.ReleaseDate
        };
        
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

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<BookDto>>> SearchBooksByTitle([FromQuery] string title)
    {
        var matchedBooks = await _dataContext.Books
            .Where(b => b.Title.ToLower().Contains(title.ToLower()))
            .Select(b => new BookDto
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
    public async Task<ActionResult<BookDto>> Get(int bookId)
    {
        var book = await _dataContext.Books.FindAsync(bookId);

        if (book is null)
        {
            return NotFound();
        }

        var bookDto = new BookDto
        {
            BookId = book.BookId,
            Title = book.Title,
            Author = book.Author,
            Publisher = book.Publisher,
            ReleaseDate = book.ReleaseDate
        };

        return Ok(bookDto);
    }

    [HttpPut("{bookId}")]
    public async Task<IActionResult> Update(int bookId, [FromBody] BookDto bookDto)
    {
        if (bookId != bookDto.BookId)
        {
            return BadRequest("Mismatched book ID.");
        }
        
        var existingBook = await _dataContext.Books.FindAsync(bookId);

        if (existingBook is null)
        {
            return NotFound();
        }
        
        existingBook.Title = bookDto.Title;
        existingBook.Author = bookDto.Author;
        existingBook.Publisher = bookDto.Publisher;
        existingBook.ReleaseDate = bookDto.ReleaseDate;
        
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
    
}