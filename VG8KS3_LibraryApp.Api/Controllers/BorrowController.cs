using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VG8KS3_LibraryApp.Api.DataBase;
using VG8KS3_LibraryApp.Api.Services;
using VG8KS3_LibraryApp.Shared.Models;

namespace VG8KS3_LibraryApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowController :ControllerBase
{
    private readonly DataContext _dataContext;

    public BorrowController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Borrow borrow)
    {
        var existingBorrow = await _dataContext.Borrows.FindAsync(borrow.BorrowId);

        if (existingBorrow is not null)
        {
            return Conflict();
        }

        var newBorrow = new Borrow()
        {
            BookId = borrow.BookId,
            ReaderId = borrow.ReaderId,
            DateOfBorrow = borrow.DateOfBorrow,
            DateOfReturn = borrow.DateOfReturn
        };


        _dataContext.Borrows.Add(newBorrow);
        await _dataContext.SaveChangesAsync();
        return Ok(newBorrow);
}
    
    [HttpDelete("{borrowId}")]
    public async Task<IActionResult> Delete(int borrowId)
    {
        var existingBorrow = await _dataContext.Borrows.FindAsync(borrowId);
            
        if (existingBorrow is null)
        {
            return NotFound();
        }
        
        _dataContext.Borrows.Remove(existingBorrow);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<Borrow>>> GetAll()
    {
        var existingBorrows = await _dataContext.Borrows.ToListAsync();
        return Ok(existingBorrows);
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Borrow>>> SearchBorrowsByBookId([FromQuery] int bookId)
    {
        var matchedBorrows = await _dataContext.Borrows
            .Where(b => b.BookId == bookId)
            .Select(b => new Borrow()
            {
                BorrowId = b.BorrowId,
                BookId = b.BookId,
                ReaderId = b.ReaderId,
                DateOfBorrow = b.DateOfBorrow,
                DateOfReturn = b.DateOfReturn
            })
            .ToListAsync();

        return Ok(matchedBorrows);
    }
    
    [HttpGet("{borrowId}")]
    public async Task<ActionResult<Borrow>> Get(int borrowId)
    {
        var existingBorrow = await _dataContext.Borrows.FindAsync(borrowId);

        if (existingBorrow is null)
        {
            return NotFound();
        }
        
        var newBorrow = new Borrow()
        {
            BorrowId = borrowId,
            BookId = existingBorrow.BookId,
            ReaderId = existingBorrow.ReaderId,
            DateOfBorrow = existingBorrow.DateOfBorrow,
            DateOfReturn = existingBorrow.DateOfReturn
        };
        
        return Ok(newBorrow);
    }

    [HttpPut("{borrowId}")]
    public async Task<IActionResult> Update(int borrowId, [FromBody] Borrow borrow)
    {
        if (borrowId != borrow.BorrowId)
        {
            return BadRequest("Mismatched borrow ID.");
        }
        
        var existingBorrow = await _dataContext.Borrows.FindAsync(borrowId);
        
        if (existingBorrow is null)
        {
            return NotFound();
        }
        
        existingBorrow.BookId = borrow.BookId;
        existingBorrow.ReaderId = borrow.ReaderId;
        existingBorrow.DateOfBorrow = borrow.DateOfBorrow;
        existingBorrow.DateOfReturn = borrow.DateOfReturn;

        await _dataContext.SaveChangesAsync();
        return NoContent();
    }
    
}