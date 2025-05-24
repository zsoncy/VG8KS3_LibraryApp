namespace VG8KS3_LibraryApp.Shared.Models;

public class BorrowDto
{
    
    public int BorrowId { get; set; }
    
    public int ReaderId { get; set; }
    
    public int BookId { get; set; }
    
    public DateTime DateOfBorrow { get; set; }
    
    public DateTime DateOfReturn { get; set; }
    
}