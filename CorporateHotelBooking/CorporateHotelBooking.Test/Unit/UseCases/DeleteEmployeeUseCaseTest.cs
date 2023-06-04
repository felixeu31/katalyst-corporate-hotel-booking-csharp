using CorporateHotelBooking.Employees.Application;
using Moq;
using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Test.Unit.UseCases
{
    public class DeleteEmployeeUseCaseTest
    {
        private readonly Mock<IEmployeeRepository> _employeeRepository;

        public DeleteEmployeeUseCaseTest()
        {
            _employeeRepository = new();
        }

        [Fact]
        public void DeleteEmployee_ShouldRemoveEmployeeFromStore()
        {
            // Arrange
            var addEmployeeUseCase = new DeleteEmployeeUseCase(_employeeRepository.Object);
            var employeeId = Guid.NewGuid();

            // Act
            addEmployeeUseCase.Execute(employeeId);

            // Assert
            _employeeRepository.Verify(x => x.Delete(It.IsAny<EmployeeId>()), Times.Once());

        }
    }
}
