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
            var addEmployeeUseCase = new AddEmployeeUseCase(_employeeRepository.Object);
            var employeeId = Guid.NewGuid();
            var companyId = Guid.NewGuid();

            // Act
            addEmployeeUseCase.Execute(companyId, employeeId);

            // Assert
            _employeeRepository.Verify(x => x.Add(It.IsAny<Employee>()), Times.Once());

        }
    }
}
