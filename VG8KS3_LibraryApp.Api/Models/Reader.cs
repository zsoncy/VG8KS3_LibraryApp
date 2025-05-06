using System.ComponentModel.DataAnnotations;

namespace VG8KS3_LibraryApp.Api.Models;

public class Reader
{
    [Key]
    public int ReaderId { get; set; }
    
    [Required(ErrorMessage = "Name is required.")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Adress is required.")]
    [MinLength(1, ErrorMessage = "Adress cannot be empty or whitespace.")]
    public string Adress { get; set; }
    
    [Required(ErrorMessage = "Birth date is required.")]
    public DateTime DateOfBirth { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DateOfBirth < new DateTime(1900, 1, 1))
        {
            yield return new ValidationResult(
                "Birth date cannot be earlier than 1900.",
                new[] { nameof(DateOfBirth) });
        }
    }

}