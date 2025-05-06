using VG8KS3_LibraryApp.Api.Models;

namespace VG8KS3_LibraryApp.Api.Services;

public class BookService : IBookService
{
    
    private readonly List<Book> _books;
    private readonly ILogger<BookService> _logger;

    public BookService(ILogger<BookService> logger)
    {
        _books = new List<Book>();
        _logger = logger;
    }
    
    
    public void Add(Book book)
    {
        _books.Add(book);
        _logger.LogInformation($"Added book: {book.Title}");
    }

    public void Update(Book book)
    {
        var oldBook = Get(book.BookId);
        oldBook.Title = book.Title;
        oldBook.Author = book.Author;
        oldBook.Publisher = book.Publisher;
        oldBook.ReleaseDate = book.ReleaseDate;
        _logger.LogInformation("Book updated");
    }
    
    public void Delete(int id)
    {
        _books.RemoveAll(b => b.BookId == id);
    }

    public List<Book> Get()
    {
        return _books;
    }

    public Book Get(int id)
    {
        return _books.Find(b => b.BookId == id);
    }
}