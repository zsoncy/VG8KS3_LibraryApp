using Microsoft.EntityFrameworkCore;
using VG8KS3_LibraryApp.Api.Models;

namespace VG8KS3_LibraryApp.Api.DataBase;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<Book> Books { get; set; }
    
    public virtual DbSet<Reader> Readers { get; set; }
    
    public virtual DbSet<Borrow> Borrows { get; set; }
    
}