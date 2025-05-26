using System.ComponentModel.DataAnnotations;
using VG8KS3_LibraryApp.Shared.Validation;

namespace VG8KS3_LibraryApp.Shared.Models;

public class Borrow
{
    
    [Key]
    public int BorrowId { get; set; }
    
    [Required(ErrorMessage = "A reader is required for a borrow.")]
    public int ReaderId { get; set; }
    
    [Required(ErrorMessage = "A book is required for a borrow.")]
    public int BookId { get; set; }
    
    [Required(ErrorMessage = "A starting date is required for a borrow.")]
    [BorrowDatesValidation]
    public DateTime DateOfBorrow { get; set; }
    
    [Required(ErrorMessage = "The estimated return date is required for a borrow.")]
    [BorrowDatesValidation]
    public DateTime DateOfReturn { get; set; }
    
}
    