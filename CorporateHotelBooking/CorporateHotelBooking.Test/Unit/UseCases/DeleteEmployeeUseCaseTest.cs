using Moq;
using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Application.Employees.UseCases;

namespace CorporateHotelBooking.Test.Unit.UseCases
{
    public class DeleteEmployeeUseCaseTest
    {
        private readonly Mock<IEmployeeRepository> _employeeRepository;
        private readonly Mock<IPoliciesRepository> _policiesRepository;
        private readonly Mock<IBookingRepository> _bokingRepository;

        public DeleteEmployeeUseCaseTest()
        {
            _employeeRepository = new();
            _policiesRepository = new();
            _bokingRepository = new();
        }

        [Fact]
        public void DeleteEmployee_ShouldRemoveEmployeeAndRelatedEntities()
        {
            // Arrange
            var addEmployeeUseCase = new DeleteEmployeeUseCase(_employeeRepository.Object, _policiesRepository.Object, _bokingRepository.Object);
            var employeeId = Guid.NewGuid();

            // Act
            addEmployeeUseCase.Execute(employeeId);

            // Assert
            _employeeRepository.Verify(x => x.Delete(It.IsAny<EmployeeId>()), Times.Once());
            _policiesRepository.Verify(x => x.DeleteEmployeePolicies(It.IsAny<EmployeeId>()), Times.Once());
            _bokingRepository.Verify(x => x.DeleteEmployeeBookings(It.IsAny<EmployeeId>()), Times.Once());

        }
    }
}
