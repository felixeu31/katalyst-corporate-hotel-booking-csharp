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

            ThenEmployeeShouldExistInRepository();

            void ThenEmployeeShouldExistInRepository()
            {
                var employeeRepository = _apiFactory.Services.GetService<IEmployeeRepository>();
                Assert.NotNull(employeeRepository.Get(EmployeeId.From(employeeId)));
            }
        }
    }
}
