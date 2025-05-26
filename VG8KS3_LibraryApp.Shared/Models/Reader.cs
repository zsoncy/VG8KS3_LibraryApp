using System.ComponentModel.DataAnnotations;
using VG8KS3_LibraryApp.Shared.Validation;

namespace VG8KS3_LibraryApp.Shared.Models;

public class Reader
{
    [Key]
    public int ReaderId { get; set; }
    
    [Required(ErrorMessage = "Name is required.")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Address is required.")]
    [MinLength(1, ErrorMessage = "Address cannot be empty or whitespace.")]
    public string Adress { get; set; }
    
    [Required(ErrorMessage = "Birth date is required.")]
    [ReaderBirthDateValidation]
    public DateTime DateOfBirth { get; set; }
    
}