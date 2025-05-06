using VG8KS3_LibraryApp.Api.Models;

namespace VG8KS3_LibraryApp.Api.Services;

public interface IBookService
{
    
    void Add(Book book);
    
    void Update(Book book);
    
    void Delete(int id);
    
    List<Book> Get();
    
    Book Get(int id);
    
}