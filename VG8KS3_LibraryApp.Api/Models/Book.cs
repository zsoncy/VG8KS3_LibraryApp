using System.ComponentModel.DataAnnotations;

namespace VG8KS3_LibraryApp.Api.Models;

public class Book
{
    
    [Key]
    public int BookId { get; set; }
    
    [Required(ErrorMessage = "Title is required.")]
    [MinLength(1, ErrorMessage = "Title cannot be empty or whitespace.")]
    public string Title { get; set; }
    
    
    [Required(ErrorMessage = "Author is required.")]
    [MinLength(1, ErrorMessage = "Author cannot be empty or whitespace.")]
    public string Author { get; set; }
    
    
    [Required(ErrorMessage = "Publisher is required.")]
    [MinLength(1, ErrorMessage = "Publisher cannot be empty or whitespace.")]
    public string Publisher { get; set; }
    
    [Required(ErrorMessage = "Release date is required.")]
    [Range(0,int.MaxValue, ErrorMessage = "The release year cannot be negative.")]
    public int ReleaseDate{ get; set; }



}