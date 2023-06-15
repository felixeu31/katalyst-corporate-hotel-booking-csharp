using CorporateHotelBooking.Data.Sql;
using Microsoft.EntityFrameworkCore;

namespace CorporateHotelBooking.Test.Fixtures.DataBase
{
    public class LocalDbTestFixture : IDisposable
    {
        private readonly string _connectionString;

        public readonly DbContextOptions<CorporateHotelDbContext> DbContextOptions;

        public LocalDbTestFixture()
        {
            // Create a unique database name
            var dbName = "CorporateHotelTestDatabase_" + Guid.NewGuid();

            // Set up the connection string for LocalDb
            _connectionString = $"Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog={dbName};Integrated Security=True";

            // Set up the DbContextOptions for the LocalDb database
            DbContextOptions = new DbContextOptionsBuilder<CorporateHotelDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            // Create the database schema using migrations
            using (var context = new CorporateHotelDbContext(DbContextOptions))
            {
                context.Database.EnsureCreated();
            }
        }

        public void Dispose()
        {
            // Delete the database
            using (var context = new CorporateHotelDbContext(DbContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}