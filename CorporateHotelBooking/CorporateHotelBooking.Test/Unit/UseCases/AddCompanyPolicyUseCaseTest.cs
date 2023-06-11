using Moq;
using CorporateHotelBooking.Test.Constants;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Application.Policies.UseCases;

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
        public void AddCompanyPolicy_ShouldInsertCompanyPolicy_WhenNotExists()
        {
            // Arrange
            var addCompanyUseCase = new AddCompanyPolicyUseCase(_policiesRepository.Object);
            var employeeId = Guid.NewGuid();
            var policies = new List<string> { RoomTypes.Standard };

            // Act
            addCompanyUseCase.Execute(employeeId, policies);

            // Assert
            _policiesRepository.Verify(x => x.AddCompanyPolicy(It.IsAny<CompanyPolicy>()), Times.Once());
        }

        [Fact]
        public void AddCompanyPolicy_ShouldUpdateCompanyPolicy_WhenAlreadyExists()
        {
            // Arrange
            var addCompanyUseCase = new AddCompanyPolicyUseCase(_policiesRepository.Object);
            var companyId = CompanyId.New();
            var policies = new List<string> { RoomTypes.Standard };
            _policiesRepository.Setup(x => x.GetCompanyPolicy(companyId)).Returns(new CompanyPolicy(companyId, new List<string>(){RoomTypes.Standard}));

            // Act
            addCompanyUseCase.Execute(companyId.Value, policies);

            // Assert
            _policiesRepository.Verify(x => x.UpdateCompanyPolicy(It.IsAny<CompanyPolicy>()), Times.Once());
        }
    }
}
