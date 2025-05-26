using System.ComponentModel.DataAnnotations;

namespace VG8KS3_LibraryApp.Shared.Validation;

public class ReaderBirthDateValidation : ValidationAttribute
{
    public ReaderBirthDateValidation()
    {
        ErrorMessage = "Birth date cannot be earlier than 1900.";
    }

    public override bool IsValid(object? value)
    {
        if (value is DateTime date)
        {
            return date >= new DateTime(1900, 1, 1);
        }

        return false; 
    }
}