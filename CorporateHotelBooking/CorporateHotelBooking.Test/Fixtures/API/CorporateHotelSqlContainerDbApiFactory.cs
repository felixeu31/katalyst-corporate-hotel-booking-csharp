using CorporateHotelBooking.Api;
using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Data.InMemory;
using CorporateHotelBooking.Data.Sql;
using CorporateHotelBooking.Data.Sql.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace CorporateHotelBooking.Test.Fixtures.API
{
    public class CorporateHotelSqlContainerDbApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        private DbContextOptions<CorporateHotelDbContext>? _dbContextOptions;
        private string? _connectionString;
        private MsSqlContainer? _msSqlContainer;

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

        public async Task InitializeAsync()
        {
            _msSqlContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithName($"CorporateHotelE2ETestDatabase_{Guid.NewGuid()}")
                .WithCleanUp(true)
                .Build();

            await _msSqlContainer.StartAsync();

            _connectionString = $"{_msSqlContainer?.GetConnectionString()}; Encrypt=False;";

            _dbContextOptions = new DbContextOptionsBuilder<CorporateHotelDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            // Create the database schema using migrations
            using (var context = new CorporateHotelDbContext(_dbContextOptions))
            {
                context.Database.EnsureCreated();
            }
        }

        public async Task DisposeAsync()
        {
            await _msSqlContainer?.StopAsync();
        }
    }
}
