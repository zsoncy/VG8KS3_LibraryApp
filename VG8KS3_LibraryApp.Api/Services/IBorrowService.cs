using VG8KS3_LibraryApp.Api.Models;

namespace VG8KS3_LibraryApp.Api.Services;

public interface IBorrowService
{
    
    void Add(Borrow borrow);
    
    void Update(Borrow borrow);
    
    void Delete(int id);
    
    List<Borrow> Get();
    
    Borrow Get(int id);
}