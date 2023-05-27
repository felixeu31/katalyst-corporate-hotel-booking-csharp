using CorporateHotelBooking.Employees.Application;
using Moq;
using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Test.Unit
{
    public class EmployeeServiceUnitTest
    {
        private readonly Mock<IEmployeeRepository> _employeeRepository;

        public EmployeeServiceUnitTest()
        {
            _employeeRepository = new();
        }

        [Fact]
        public void AddEmployee_ShouldStoreEmployee()
        {
            // Arrange
            var employeeService = new EmployeeService(_employeeRepository.Object);
            var employeeId = Guid.NewGuid();
            var companyId = 1;

            // Act
            employeeService.AddEmployee(companyId, employeeId);

            // Assert
            _employeeRepository.Verify(x => x.Add(It.IsAny<Employee>()), Times.Once());

        }
    }
}
