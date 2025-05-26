using Microsoft.EntityFrameworkCore;
using VG8KS3_LibraryApp.Api.DataBase;

namespace VG8KS3_LibraryApp_Api.Tests.TestUtilities

{

    public static class TestDbContextFactory
    {
        public static DataContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new DataContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
    
}
