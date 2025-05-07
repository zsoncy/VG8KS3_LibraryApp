using Microsoft.AspNetCore.Mvc;
using VG8KS3_LibraryApp.Api.Models;
using VG8KS3_LibraryApp.Api.Services;

namespace VG8KS3_LibraryApp.Api.Controllers;

[ApiController]
[Route("book")]
public class BookController: ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public IActionResult Add([FromBody] Book book)
    {
        var existingBook = _bookService.Get(book.BookId);

        if (existingBook is not null)
        {
            return Conflict();
        }
        
        _bookService.Add(book);
        return Ok();
    }

    [HttpDelete("{bookId}")]
    public IActionResult Delete(int BookId)
    {
        var existingBook = _bookService.Get(BookId);

        if (existingBook is null)
        {
            return NotFound();
        }
        
        _bookService.Delete(BookId);
        return Ok();
    }

    [HttpGet]
    public ActionResult<List<Book>> GetAll()
    {
        var books = _bookService.Get();
        return Ok(books);
    }

    [HttpGet("{BookId}")]
    public ActionResult<Book> Get(int BookId)
    {
        var book = _bookService.Get(BookId);
        
        if (book is null)
        {
            return NotFound();
        }
        
        return Ok(book);
    }

    [HttpPut("{BookId}")]
    public IActionResult Update(int BookId, [FromBody] Book book)
    {
        if (BookId != book.BookId)
        {
            return BadRequest();
        }
        
        var oldBook = _bookService.Get(BookId);

        if (oldBook is null)
        {
            return NotFound();
        }
        
        _bookService.Update(book);
        return Ok();
    }
    
}