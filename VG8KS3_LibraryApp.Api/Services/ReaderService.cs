using VG8KS3_LibraryApp.Api.Models;

namespace VG8KS3_LibraryApp.Api.Services;

public class ReaderService : IReaderService
{
    
    private readonly List<Reader> _readers;
    private readonly ILogger<ReaderService> _logger;

    public ReaderService(ILogger<ReaderService> logger)
    {
        _readers = new List<Reader>();
        _logger = logger;
    }
    
    public void Add(Reader reader)
    {
        _readers.Add(reader);
        _logger.LogInformation($"Added reader: {reader.Name}");
    }

    public void Update(Reader reader)
    {
        var oldReader = Get(reader.ReaderId);
        oldReader.Name = reader.Name;
        oldReader.Adress = reader.Adress;
        oldReader.DateOfBirth = reader.DateOfBirth;
        _logger.LogInformation($"Reader updated: {reader.Name}");
    }

    public void Delete(int id)
    {
        _readers.RemoveAll(r=>r.ReaderId == id);
        _logger.LogInformation("Reader removed");
    }

    public List<Reader> Get()
    {
        return _readers;
    }

    public Reader Get(int id)
    {
        return _readers.Find(r=>r.ReaderId == id);
    }
    
}