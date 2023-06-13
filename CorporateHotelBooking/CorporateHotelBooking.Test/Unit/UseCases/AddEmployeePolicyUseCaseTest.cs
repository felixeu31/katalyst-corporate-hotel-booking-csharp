using Moq;
using System.ComponentModel.Design;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Application.Policies.UseCases;
using CorporateHotelBooking.Test.TestUtilities;
using CorporateHotelBooking.Test.Constants;

namespace CorporateHotelBooking.Test.Unit.UseCases;

[Trait(TestTrait.Category, TestCategory.Unit)]
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
        var policies = new List<string> { SampleData.RoomTypes.Standard };

        // Act
        addEmployeeUseCase.Execute(employeeId, policies);

        // Assert
        _policiesRepository.Verify(x => x.AddEmployeePolicy(It.IsAny<EmployeePolicy>()), Times.Once());
    }

    [Fact]
    public void AddEmployeePolicy_ShouldUpdateEmployeePolicy_WhenAlreadyExists()
    {
        // Arrange
        var addEmployeeUseCase = new AddEmployeePolicyUseCase(_policiesRepository.Object);
        var employeeId = EmployeeId.New();
        var policies = new List<string> { SampleData.RoomTypes.Standard };
        _policiesRepository.Setup(x => x.GetEmployeePolicy(employeeId)).Returns(new EmployeePolicy(employeeId, new List<string>() { SampleData.RoomTypes.Standard }));

        // Act
        addEmployeeUseCase.Execute(employeeId.Value, policies);

        // Assert
        _policiesRepository.Verify(x => x.UpdateEmployeePolicy(It.IsAny<EmployeePolicy>()), Times.Once());
    }
}