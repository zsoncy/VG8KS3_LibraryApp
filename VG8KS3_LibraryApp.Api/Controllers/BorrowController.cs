using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VG8KS3_LibraryApp.Api.DataBase;
using VG8KS3_LibraryApp.Api.Models;
using VG8KS3_LibraryApp.Api.Services;

namespace VG8KS3_LibraryApp.Api.Controllers;

[ApiController]
[Route("borrow")]
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
    public async Task<ActionResult<List<Borrow>>> GetAll()
    {
        var existingBorrows = await _dataContext.Borrows.ToListAsync();
        return Ok(existingBorrows);
    }

    [HttpGet("{borrowId}")]
    public async Task<ActionResult<Borrow>> Get(int borrowId)
    {
        var existingBorrow = await _dataContext.Borrows.FindAsync(borrowId);

        if (existingBorrow is null)
        {
            return NotFound();
        }
        
        return Ok(existingBorrow);
    }

    [HttpPut("{borrowId}")]
    public async Task<IActionResult> Update(int borrowId, [FromBody] Borrow borrow)
    {
        if (borrowId != borrow.BorrowId)
        {
            return BadRequest();
        }
        
        var existingBorrow = await _dataContext.Borrows.FindAsync(borrowId);
        
        if (existingBorrow is null)
        {
            return NotFound();
        }
        
        _dataContext.Borrows.Update(existingBorrow);
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
    
}