using VG8KS3_LibraryApp.Api.Models;

namespace VG8KS3_LibraryApp.Api.Services;

public class BorrowService: IBorrowService
{
    
    private readonly List<Borrow> _borrows;
    private readonly ILogger<BookService> _logger;

    public BorrowService(ILogger<BookService> logger)
    {
        _borrows = new List<Borrow>();
        _logger = logger;
    }
    
    
    public void Add(Borrow borrow)
    {
        _borrows.Add(borrow);
        _logger.LogInformation($"Borrow added: {borrow}");
    }

    public void Update(Borrow borrow)
    {
        var oldBorrow = Get(borrow.BorrowId);
        oldBorrow.BookId = borrow.BookId;
        oldBorrow.ReaderId = borrow.ReaderId;
        oldBorrow.DateOfBorrow = borrow.DateOfBorrow;
        oldBorrow.DateOfReturn = borrow.DateOfReturn;
        _logger.LogInformation($"Borrow updated");
    }

    public void Delete(int id)
    {
        _borrows.RemoveAll(borrow => borrow.BorrowId == id);
        _logger.LogInformation($"Borrow deleted");
    }

    public List<Borrow> Get()
    {
        return _borrows;
    }

    public Borrow Get(int id)
    {
        return _borrows.Find(borrow => borrow.BorrowId == id);
    }
}