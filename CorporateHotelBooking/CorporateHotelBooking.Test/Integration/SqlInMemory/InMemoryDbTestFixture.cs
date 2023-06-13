using CorporateHotelBooking.Data.Sql;
using Microsoft.EntityFrameworkCore;

namespace CorporateHotelBooking.Test.Integration.Sql;

public class InMemoryDbTestFixture : IDisposable
{
    public readonly DbContextOptions<CorporateHotelDbContext> DbContextOptions;

    public InMemoryDbTestFixture()
    {
        // Set up the DbContextOptions for the in-memory database
        DbContextOptions = new DbContextOptionsBuilder<CorporateHotelDbContext>()
            .UseInMemoryDatabase("CorporateHotelTestDatabase")
            .Options;
    }

    public void Dispose()
    {
    }
}