using CorporateHotelBooking.Employees.Application;
using Moq;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Policies.Application;
using CorporateHotelBooking.Policies.Domain;

namespace CorporateHotelBooking.Test.Unit.UseCases
{
    public class AddCompanyPolicyUseCaseTest
    {
        private readonly Mock<IPoliciesRepository> _policiesRepository;

        public AddCompanyPolicyUseCaseTest()
        {
            _policiesRepository = new();
        }

        [Fact]
        public void AddCompanyPolicy_ShouldStoreCompany()
        {
            // Arrange
            var addCompanyUseCase = new AddCompanyPolicyUseCase(_policiesRepository.Object);
            var employeeId = Guid.NewGuid();
            var policies = new List<string> { "Standard" };

            // Act
            addCompanyUseCase.Execute(employeeId, policies);

            // Assert
            _policiesRepository.Verify(x => x.AddCompanyPolicy(It.IsAny<CompanyPolicy>()), Times.Once());
        }
    }
}
