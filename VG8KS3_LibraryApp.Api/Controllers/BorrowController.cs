using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VG8KS3_LibraryApp.Api.DataBase;
using VG8KS3_LibraryApp.Api.Models;
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
    public async Task<IActionResult> Add([FromBody] BorrowDto borrowDto)
    {
        var existingBorrow = await _dataContext.Borrows.FindAsync(borrowDto.BorrowId);

        if (existingBorrow is not null)
        {
            return Conflict();
        }
        
        var borrow = new Borrow()
        {
            BookId = borrowDto.BookId,
            ReaderId = borrowDto.ReaderId,
            DateOfBorrow = borrowDto.DateOfBorrow,
            DateOfReturn = borrowDto.DateOfReturn
        };
        
        
        _dataContext.Borrows.Add(borrow);
        await _dataContext.SaveChangesAsync();
        return Ok();
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
    public async Task<ActionResult<List<BorrowDto>>> GetAll()
    {
        var existingBorrows = await _dataContext.Borrows.ToListAsync();
        return Ok(existingBorrows);
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<BorrowDto>>> SearchBorrowsByBookId([FromQuery] int bookId)
    {
        var matchedBorrows = await _dataContext.Borrows
            .Where(b => b.BookId == bookId)
            .Select(b => new BorrowDto()
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
    public async Task<ActionResult<BorrowDto>> Get(int borrowId)
    {
        var existingBorrow = await _dataContext.Borrows.FindAsync(borrowId);

        if (existingBorrow is null)
        {
            return NotFound();
        }
        
        var borrowDto = new BorrowDto()
        {
            BorrowId = borrowId,
            BookId = existingBorrow.BookId,
            ReaderId = existingBorrow.ReaderId,
            DateOfBorrow = existingBorrow.DateOfBorrow,
            DateOfReturn = existingBorrow.DateOfReturn
        };
        
        return Ok(borrowDto);
    }

    [HttpPut("{borrowId}")]
    public async Task<IActionResult> Update(int borrowId, [FromBody] BorrowDto borrowDto)
    {
        if (borrowId != borrowDto.BorrowId)
        {
            return BadRequest("Mismatched borrow ID.");
        }
        
        var existingBorrow = await _dataContext.Borrows.FindAsync(borrowId);
        
        if (existingBorrow is null)
        {
            return NotFound();
        }
        
        existingBorrow.BookId = borrowDto.BookId;
        existingBorrow.ReaderId = borrowDto.ReaderId;
        existingBorrow.DateOfBorrow = borrowDto.DateOfBorrow;
        existingBorrow.DateOfReturn = borrowDto.DateOfReturn;

        await _dataContext.SaveChangesAsync();
        return Ok();
    }
    
}