using CorporateHotelBooking.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CorporateHotelBooking.Test.Fixtures.API
{
    public class CorporateHotelInMemoryApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
