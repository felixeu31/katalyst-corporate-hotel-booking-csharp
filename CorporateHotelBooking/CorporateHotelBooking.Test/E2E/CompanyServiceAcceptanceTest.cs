using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Hotels.Domain;
using CorporateHotelBooking.Test.ApiFactory;
using System.Net;
using System.Net.Http.Json;
using CorporateHotelBooking.Employees.Infra;
using Microsoft.Extensions.DependencyInjection;
using CorporateHotelBooking.Data;

namespace CorporateHotelBooking.Test.E2E
{
    // TODO: When deleting an employee, all the bookings and policies associated to the employee should also be deleted from the system.

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
                var context = _apiFactory.Services.GetService<InMemoryContext>();
                Assert.NotNull(context.Employees[EmployeeId.From(employeeId)]);
            }
        }

        [Fact]
        public async void should_be_able_to_delete_an_employee()
        {
            // Arrange
            Guid employeeId = Guid.NewGuid();
            Guid companyId = Guid.NewGuid();
            var givenEmployeeRepository = GivenRepositoryWithExistingEmployee();

            // Act
            var addEmployeeResponse = await _client.DeleteAsync($"employees/{employeeId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, addEmployeeResponse.StatusCode);

            ThenEmployeeShouldNotExistInRepository();

            IEmployeeRepository GivenRepositoryWithExistingEmployee()
            {
                var employeeRepository = _apiFactory.Services.GetService<IEmployeeRepository>();

                employeeRepository.Add(new Employee(CompanyId.From(companyId), EmployeeId.From(employeeId)));

                return employeeRepository;
            }

            void ThenEmployeeShouldNotExistInRepository()
            {
                Assert.Null(givenEmployeeRepository.Get(EmployeeId.From(employeeId)));
            }
        }
    }
}
