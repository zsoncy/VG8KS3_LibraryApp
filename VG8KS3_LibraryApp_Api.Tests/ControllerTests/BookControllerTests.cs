using Xunit;
using VG8KS3_LibraryApp.Api.Controllers;
using VG8KS3_LibraryApp.Api.DataBase;
using VG8KS3_LibraryApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using VG8KS3_LibraryApp_Api.Tests.TestUtilities;

public class BooksControllerTests
{
    private readonly BookController _controller;
    private readonly DataContext _context;

    public BooksControllerTests()
    {
        _context = TestDbContextFactory.CreateInMemoryDbContext();
        SeedDatabase(_context);
        _controller = new BookController(_context);
    }

    private void SeedDatabase(DataContext context)
    {
        context.Books.AddRange(new List<Book>
        {
            new Book { Title = "Test Book 1", Author = "Author A", Publisher = "Publisher A", ReleaseDate = 1980 },
            new Book { Title = "Test Book 2", Author = "Author B", Publisher = "Publisher M", ReleaseDate = 2001 },
            new Book { Title = "Test Book 3", Author = "Author C", Publisher = "Publisher G", ReleaseDate = 2016 },
            new Book { Title = "Test Book 4", Author = "Author D", Publisher = "Publisher E", ReleaseDate = 2009 },
            new Book { Title = "Test Book 5", Author = "Author E", Publisher = "Publisher D", ReleaseDate = 2009 }
        });
        context.SaveChanges();
    }

    [Fact]
    public async Task GetBooks_ReturnsAllBooks()
    {
        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
        Assert.Equal(5, books.Count());
    }
    
    [Fact]
    public async Task AddBook_ValidBook_ReturnsCreatedBook()
    {
        var newBook = new Book()
        {
            Title = "New Test Book",
            Author = "New Test Author",
            Publisher = "New Test Publisher",
            ReleaseDate = 987654321
        };

        var result = await _controller.Add(newBook);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedBook = Assert.IsType<Book>(createdResult.Value);
        
        Assert.Equal(newBook.Title, returnedBook.Title);
        Assert.Equal(newBook.Author, returnedBook.Author);
        Assert.Equal(newBook.Publisher, returnedBook.Publisher);
        Assert.Equal(newBook.ReleaseDate, returnedBook.ReleaseDate);
    }
    
    [Fact]
    public async Task ExistingBook_UpdateBook_ReturnsUpdatedBook()
    {
        var existingBook = _context.Books.First();
        
        var updatedBook = new Book()
        {
            BookId = existingBook.BookId,
            Title = "Updated Title?>Title>?",
            Author = existingBook.Author,
            Publisher = existingBook.Publisher,
            ReleaseDate = existingBook.ReleaseDate
        };

        var result = await _controller.Update(existingBook.BookId, updatedBook);
        var bookInDb = await _context.Books.FindAsync(existingBook.BookId);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal("Updated Title?>Title>?", bookInDb.Title);
        
    }
    
    [Fact]
    public async Task DataBaseNumber_DeleteBook_ReturnsOneLess()
    {
        var result = await _controller.GetAll();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
        int dataBaseNumber = books.Count();
        
        await _controller.Delete(books.First().BookId);
        
        var updatedResult = await _controller.GetAll();
        var updatedOkResult = Assert.IsType<OkObjectResult>(updatedResult.Result);
        var updatedBooks = Assert.IsAssignableFrom<IEnumerable<Book>>(updatedOkResult.Value);
        int updatedDataBaseNumber = updatedBooks.Count();
        
        Assert.Equal(dataBaseNumber-1, updatedDataBaseNumber);
        
    }
    
}