using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Employees.Infra;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Unit
{
    public class InMemoryEmployeeRepositoryTest
    {
        private readonly IEmployeeRepository _employeeRepository;

        public InMemoryEmployeeRepositoryTest()
        {
            _employeeRepository = new InMemoryEmployeeRepository();
        }

        [Fact]
        public void should_retrieve_added_employee()
        {
            // Arrange
            int employeeId = 1;
            int companyId = 1;

            // Act
            _employeeRepository.Add(new Employee(companyId, employeeId));
            Employee employee = _employeeRepository.Get(employeeId);

            // Assert
            employee.Should().NotBeNull();
            employee.CompanyId.Should().Be(1);
            employee.EmployeeId.Should().Be(1);
        }

    }
}
