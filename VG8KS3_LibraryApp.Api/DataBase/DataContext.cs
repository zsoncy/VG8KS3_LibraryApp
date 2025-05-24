using Microsoft.EntityFrameworkCore;
using VG8KS3_LibraryApp.Api.Models;
using VG8KS3_LibraryApp.Shared.Models;

namespace VG8KS3_LibraryApp.Api.DataBase;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<BookDto> Books { get; set; }
    
    public virtual DbSet<ReaderDto> Readers { get; set; }
    
    public virtual DbSet<BorrowDto> Borrows { get; set; }
    
}