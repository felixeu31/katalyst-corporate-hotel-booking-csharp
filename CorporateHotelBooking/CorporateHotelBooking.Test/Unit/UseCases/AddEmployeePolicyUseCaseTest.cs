using CorporateHotelBooking.Employees.Application;
using Moq;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Policies.Application;
using CorporateHotelBooking.Policies.Domain;

namespace CorporateHotelBooking.Test.Unit.UseCases
{
    public class AddEmployeePolicyUseCaseTest
    {
        private readonly Mock<IPoliciesRepository> _policiesRepository;

        public AddEmployeePolicyUseCaseTest()
        {
            _policiesRepository = new();
        }

        [Fact]
        public void AddEmployeePolicy_ShouldStoreEmployee()
        {
            // Arrange
            var addEmployeeUseCase = new AddEmployeePolicyUseCase(_policiesRepository.Object);
            var employeeId = Guid.NewGuid();
            var policies = new List<string> { "Standard" };

            // Act
            addEmployeeUseCase.Execute(employeeId, policies);

            // Assert
            _policiesRepository.Verify(x => x.AddEmployeePolicy(It.IsAny<EmployeeId>(), It.IsAny<List<string>>()), Times.Once());
        }
    }
}
