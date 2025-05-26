using Xunit;
using VG8KS3_LibraryApp.Api.Controllers;
using VG8KS3_LibraryApp.Api.DataBase;
using VG8KS3_LibraryApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using VG8KS3_LibraryApp_Api.Tests.TestUtilities;

namespace VG8KS3_LibraryApp_Api.Tests.ControllerTests;

public class BorrowControllerTest
{
    private readonly BorrowController _controller;
    private readonly DataContext _context;
    
    public BorrowControllerTest()
    {
        _context = TestDbContextFactory.CreateInMemoryDbContext();
        SeedDatabase(_context);
        _controller = new BorrowController(_context);
    }
    
    private void SeedDatabase(DataContext context)
    {
        context.Borrows.AddRange(new List<Borrow>
        {
            new Borrow { BookId = 1, ReaderId = 1, DateOfBorrow = DateTime.Today, DateOfReturn = DateTime.Today.AddDays(12) },
            new Borrow { BookId = 2, ReaderId = 1, DateOfBorrow = DateTime.Today.AddDays(2), DateOfReturn = DateTime.Today.AddDays(20) },
            new Borrow { BookId = 3, ReaderId = 2, DateOfBorrow = DateTime.Today, DateOfReturn = DateTime.Today.AddDays(12) },
            new Borrow { BookId = 4, ReaderId = 3, DateOfBorrow = DateTime.Today.AddDays(10), DateOfReturn = DateTime.Today.AddDays(30) },
            new Borrow { BookId = 5, ReaderId = 3, DateOfBorrow = DateTime.Today.AddDays(10), DateOfReturn = DateTime.Today.AddDays(60) }
        });
        context.SaveChanges();
    }
    
    [Fact]
    public async Task HaveBorrows_GetAllBorrows_ReturnsAllBorrows()
    {
        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var borrows = Assert.IsAssignableFrom<IEnumerable<Borrow>>(okResult.Value);
        Assert.Equal(5, borrows.Count());
    }
    
    [Fact]
    public async Task ValidBorrow_AddBorrow_ReturnsCreatedBorrow()
    {
        var newBorrow = new Borrow()
        {
            BookId = 1,
            ReaderId = 1,
            DateOfBorrow = DateTime.Today,
            DateOfReturn = DateTime.Today.AddDays(22)
        };

        var result = await _controller.Add(newBorrow);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedBorrow = Assert.IsType<Borrow>(okResult.Value);
        
        Assert.Equal(newBorrow.BookId, returnedBorrow.BookId);
        Assert.Equal(newBorrow.ReaderId, returnedBorrow.ReaderId);
        Assert.Equal(newBorrow.DateOfReturn, returnedBorrow.DateOfReturn);
        Assert.Equal(newBorrow.DateOfReturn, returnedBorrow.DateOfReturn);
    }
    
    [Fact]
    public async Task ExistingBorrow_UpdateBorrow_ReturnsUpdatedBorrow()
    {
        var existingBorrow = _context.Borrows.First();
        
        var updatedBorrow = new Borrow()
        {
            BorrowId = existingBorrow.BorrowId,
            BookId = existingBorrow.BookId,
            ReaderId = 9999,
            DateOfBorrow = existingBorrow.DateOfReturn,
            DateOfReturn = existingBorrow.DateOfReturn
        };

        var result = await _controller.Update(existingBorrow.BorrowId, updatedBorrow);
        var BorrowInDb = await _context.Borrows.FindAsync(existingBorrow.BorrowId);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(9999, BorrowInDb.ReaderId);
        
    }
    
    [Fact]
    public async Task DataBaseNumber_DeleteBorrow_ReturnsOneLess()
    {
        var result = await _controller.GetAll();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var borrows = Assert.IsAssignableFrom<IEnumerable<Borrow>>(okResult.Value);
        int dataBaseNumber = borrows.Count();
        
        await _controller.Delete(borrows.First().BorrowId);
        
        var updatedResult = await _controller.GetAll();
        var updatedOkResult = Assert.IsType<OkObjectResult>(updatedResult.Result);
        var updatedBorrows = Assert.IsAssignableFrom<IEnumerable<Borrow>>(updatedOkResult.Value);
        int updatedDataBaseNumber = updatedBorrows.Count();
        
        Assert.Equal(dataBaseNumber-1, updatedDataBaseNumber);
        
    }
}