using Moq;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Employees.UseCases;
using CorporateHotelBooking.Test.Constants;

namespace CorporateHotelBooking.Test.Unit.UseCases;

[Trait(TestTrait.Category, TestCategory.Unit)]
public class AddEmployeeUseCaseTest
{
    private readonly Mock<IEmployeeRepository> _employeeRepository;

    public AddEmployeeUseCaseTest()
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