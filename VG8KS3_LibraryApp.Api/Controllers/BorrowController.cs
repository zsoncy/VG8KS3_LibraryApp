using Microsoft.AspNetCore.Mvc;
using VG8KS3_LibraryApp.Api.Models;
using VG8KS3_LibraryApp.Api.Services;

namespace VG8KS3_LibraryApp.Api.Controllers;

[ApiController]
[Route("borrow")]
public class BorrowController :ControllerBase
{
    private readonly IBorrowService _borrowService;

    public BorrowController(IBorrowService borrowService)
    {
        _borrowService = borrowService;
    }

    [HttpPost]
    public IActionResult Add([FromBody] Borrow borrow)
    {
        var existingBorrow = _borrowService.Get(borrow.BorrowId);

        if (existingBorrow is not null)
        {
            return Conflict();
        }
        
        _borrowService.Add(borrow);
        return Ok();
    }
    
    [HttpDelete("{borrowId}")]
    public IActionResult Delete(int borrowId)
    {
        var existingBorrow = _borrowService.Get(borrowId);

        if (existingBorrow is null)
        {
            return NotFound();
        }
        
        _borrowService.Delete(borrowId);
        return Ok();
    }

    [HttpGet]
    public ActionResult<List<Borrow>> GetAll()
    {
        var existingBorrows = _borrowService.Get();
        return Ok(existingBorrows);
    }

    [HttpGet("{borrowId}")]
    public ActionResult<Borrow> Get(int borrowId)
    {
        var existingBorrow = _borrowService.Get(borrowId);

        if (existingBorrow is null)
        {
            return NotFound();
        }
        
        return Ok(existingBorrow);
    }

    [HttpPut("{borrowId}")]
    public IActionResult Update(int borrowId, [FromBody] Borrow borrow)
    {
        if (borrowId != borrow.BorrowId)
        {
            return BadRequest();
        }
        
        var existingBorrow = _borrowService.Get(borrow.BorrowId);
        
        if (existingBorrow is null)
        {
            return NotFound();
        }
        
        _borrowService.Update(borrow);
        return Ok();
    }
    
}