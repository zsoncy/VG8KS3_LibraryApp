using System.ComponentModel.DataAnnotations;

namespace VG8KS3_LibraryApp.Api.Models;

public class Borrow
{
    
    [Key]
    public int BorrowId { get; set; }
    
    [Required(ErrorMessage = "A reader is required for a borrow.")]
    public int ReaderId { get; set; }
    
    [Required(ErrorMessage = "A book is required for a borrow.")]
    public int BookId { get; set; }
    
    [Required(ErrorMessage = "A starting date is required for a borrow.")]
    public DateTime DateOfBorrow { get; set; }
    
    
    public DateTime DateOfReturn { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DateOfBorrow < DateTime.Now)
        {
            yield return new ValidationResult(
                "The starting date of the borrowing cannot be earlier than the current date.",
                new[] { nameof(DateOfBorrow) });
        }

        if (DateOfReturn < DateOfBorrow)
        {
            yield return new ValidationResult(
                "The ending date of the borrowing cannot be earlier than it's starting date.",
                new[] { nameof(DateOfReturn) });
        }
    }
}