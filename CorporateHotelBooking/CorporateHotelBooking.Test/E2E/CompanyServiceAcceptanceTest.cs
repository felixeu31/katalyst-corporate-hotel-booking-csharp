using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Hotels.Domain;
using CorporateHotelBooking.Test.ApiFactory;
using System.Net;
using System.Net.Http.Json;
using CorporateHotelBooking.Employees.Infra;
using Microsoft.Extensions.DependencyInjection;

namespace CorporateHotelBooking.Test.E2E
{
    public class CompanyServiceAcceptanceTest : IClassFixture<CorporateHotelApiFactory>
    {
        private readonly CorporateHotelApiFactory _apiFactory;
        private readonly HttpClient _client;

        public CompanyServiceAcceptanceTest(CorporateHotelApiFactory apiFactory)
        {
            _apiFactory = apiFactory;
            _client = apiFactory.CreateClient();
        }

        [Fact]
        public async void should_be_able_to_add_employee()
        {
            // Arrange
            Guid employeeId = Guid.NewGuid();
            Guid companyId = Guid.NewGuid();

            // Act
            var addEmployeeResponse = await _client.PostAsJsonAsync("employees", new { CompanyId = companyId, EmployeeId = employeeId });

            // Assert
            Assert.Equal(HttpStatusCode.Created, addEmployeeResponse.StatusCode);
            
            // Just and example that could help preventing the access to the repo from
            // the acceptance test.
            // The treadoff here is that I am assuming that we will at some point in time have a get endpoint anyways.
            // So given that this is true, this acceptance test would then test even more the outside-part, then
            // in the inside you can actually assert the repo.
            var listEmployeeResponse = await _client.GetAsJsonAsync("employees", new { CompanyId = companyId, EmployeeId = employeeId });

            Assert.NotNull(listEmployeeResponse.Get(EmployeeId.From(employeeId)));
        }
    }
}
