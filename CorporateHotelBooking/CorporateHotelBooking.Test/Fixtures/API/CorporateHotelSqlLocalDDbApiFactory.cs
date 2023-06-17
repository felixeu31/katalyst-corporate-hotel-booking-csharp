using CorporateHotelBooking.Api;
using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Data.InMemory;
using CorporateHotelBooking.Data.InMemory.Repositories;
using CorporateHotelBooking.Data.Sql;
using CorporateHotelBooking.Data.Sql.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CorporateHotelBooking.Test.Fixtures.API
{
    public class CorporateHotelSqlLocalDbApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        private DbContextOptions<CorporateHotelDbContext>? _dbContextOptions;
        private string? _connectionString;


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(IBookingRepository));
                services.RemoveAll(typeof(IHotelRepository));
                services.RemoveAll(typeof(IEmployeeRepository));
                services.RemoveAll(typeof(IPoliciesRepository));
                services.RemoveAll(typeof(InMemoryContext));

                services.AddScoped<IBookingRepository, SqlBookingRepository>();
                services.AddScoped<IHotelRepository, SqlHotelRepository>();
                services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();
                services.AddScoped<IPoliciesRepository, SqlPoliciesRepository>();

                services.AddDbContext<CorporateHotelDbContext>(options =>
                {
                    options.UseSqlServer(_connectionString);
                });
            });
        }

        public Task InitializeAsync()
        {
            var dbName = $"CorporateHotelE2ETestDatabase_{Guid.NewGuid()}";
            _connectionString = $"Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog={dbName};Integrated Security=True";

            _dbContextOptions = new DbContextOptionsBuilder<CorporateHotelDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            // Create the database schema using migrations
            using (var context = new CorporateHotelDbContext(_dbContextOptions))
            {
                context.Database.EnsureCreated();
            }

            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            // Delete the database
            using (var context = new CorporateHotelDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
            }

            return Task.CompletedTask;
        }
    }
}
