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

public class ReaderControllerTests
{
    private readonly ReaderController _controller;
    private readonly DataContext _context;
    
    public ReaderControllerTests()
    {
        _context = TestDbContextFactory.CreateInMemoryDbContext();
        SeedDatabase(_context);
        _controller = new ReaderController(_context);
    }
    
    private void SeedDatabase(DataContext context)
    {
        context.Readers.AddRange(new List<Reader>
        {
            new Reader { Name = "Test Name 1", Adress = "Test Address 1", DateOfBirth = DateTime.Today.AddYears(-20) },
            new Reader { Name = "Test Name 2", Adress = "Test Address 4", DateOfBirth = DateTime.Today.AddYears(-51) },
            new Reader { Name = "Test Name 3", Adress = "Test Address 55", DateOfBirth = DateTime.Today.AddYears(-17) },
            new Reader { Name = "Test Name 4", Adress = "Test Address 9", DateOfBirth = DateTime.Today.AddYears(-32) },
            new Reader { Name = "Test Name 5", Adress = "Test Address 3", DateOfBirth = DateTime.Today.AddYears(-32) }
        });
        context.SaveChanges();
    }
    
    [Fact]
    public async Task HaveReaders_GetAllReaders_ReturnsAllReaders()
    {
        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var readers = Assert.IsAssignableFrom<IEnumerable<Reader>>(okResult.Value);
        Assert.Equal(5, readers.Count());
    }
    
    [Fact]
    public async Task ValidReader_AddReader_ReturnsCreatedReader()
    {
        var newReader = new Reader()
        {
            Name = "New Test Reader's Name",
            Adress = "New Test Reader's Address street 12. number", 
            DateOfBirth = DateTime.Today.AddYears(-36)
        };

        var result = await _controller.Add(newReader);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedReader = Assert.IsType<Reader>(createdResult.Value);
        
        Assert.Equal(newReader.Name, returnedReader.Name);
        Assert.Equal(newReader.Adress, returnedReader.Adress);
        Assert.Equal(newReader.DateOfBirth, returnedReader.DateOfBirth);
    }
    
    [Fact]
    public async Task ExistingReader_UpdateReader_ReturnsUpdatedReader()
    {
        var existingReader = _context.Readers.First();
        
        var updatedReader = new Reader()
        {
            ReaderId = existingReader.ReaderId,
            Name = existingReader.Name,
            Adress = "Updated Address?>New Address>? 22. number",
            DateOfBirth = existingReader.DateOfBirth
        };

        var result = await _controller.Update(existingReader.ReaderId, updatedReader);
        var readerInDb = await _context.Readers.FindAsync(existingReader.ReaderId);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal("Updated Address?>New Address>? 22. number", readerInDb.Adress);
        
    }
    
    [Fact]
    public async Task DataBaseNumber_DeleteReader_ReturnsOneLess()
    {
        var result = await _controller.GetAll();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var readers = Assert.IsAssignableFrom<IEnumerable<Reader>>(okResult.Value);
        int dataBaseNumber = readers.Count();
        
        await _controller.Delete(readers.First().ReaderId);
        
        var updatedResult = await _controller.GetAll();
        var updatedOkResult = Assert.IsType<OkObjectResult>(updatedResult.Result);
        var updatedReaders = Assert.IsAssignableFrom<IEnumerable<Reader>>(updatedOkResult.Value);
        int updatedDataBaseNumber = updatedReaders.Count();
        
        Assert.Equal(dataBaseNumber-1, updatedDataBaseNumber);
        
    }

}