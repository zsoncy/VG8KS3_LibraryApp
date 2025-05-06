using VG8KS3_LibraryApp.Api.Models;

namespace VG8KS3_LibraryApp.Api.Services;

public interface IReaderService
{
    void Add(Reader reader);
    
    void Update(Reader reader);
    
    void Delete(int id);
    
    List<Reader> Get();
    
    Reader Get(int id);
}