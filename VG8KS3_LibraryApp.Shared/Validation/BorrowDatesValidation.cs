using System.ComponentModel.DataAnnotations;
using VG8KS3_LibraryApp.Shared.Models;

namespace VG8KS3_LibraryApp.Shared.Validation;

public class BorrowDatesValidation: ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is not Borrow borrow)
            return ValidationResult.Success!;

        if (borrow.DateOfBorrow < DateTime.Today)
        {
            return new ValidationResult(
                "The starting date of the borrowing cannot be earlier than today.",
                new[] { nameof(Borrow.DateOfBorrow) });
        }

        if (borrow.DateOfReturn < borrow.DateOfBorrow)
        {
            return new ValidationResult(
                "The return date cannot be earlier than the borrow date.",
                new[] { nameof(Borrow.DateOfReturn) });
        }

        return ValidationResult.Success!;
    }
}