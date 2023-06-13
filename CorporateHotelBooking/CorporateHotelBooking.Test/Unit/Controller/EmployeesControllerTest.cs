using CorporateHotelBooking.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using FluentAssertions;
using CorporateHotelBooking.Application.Employees.UseCases;
using CorporateHotelBooking.Test.Constants;

namespace CorporateHotelBooking.Test.Unit.Controller;

[Trait(TestTrait.Category, TestCategory.Unit)]
public class EmployeesControllerTest
{
    private readonly Mock<IAddEmployeeUseCase> _addEmployeeUseCaseMock;
    private readonly Mock<IDeleteEmployeeUseCase> _deleteEmployeeUseCaseMock;
    private readonly EmployeesController _employeesController;

    public EmployeesControllerTest()
    {
        _addEmployeeUseCaseMock = new Mock<IAddEmployeeUseCase>();
        _deleteEmployeeUseCaseMock = new Mock<IDeleteEmployeeUseCase>();
        _employeesController = new EmployeesController(_addEmployeeUseCaseMock.Object, _deleteEmployeeUseCaseMock.Object);
    }

    [Fact]
    public void AddEmployee_ShouldProcessRequestAndInvokeUseCase()
    {
        // Arrange
        var addEmployeeData = new AddEmployeeData(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var addEmployeeResponse = _employeesController.AddEmployee(addEmployeeData);

        // Assert
        _addEmployeeUseCaseMock.Verify(mock => mock.Execute(addEmployeeData.CompanyId, addEmployeeData.EmployeeId), Times.Once);
        ((StatusCodeResult)addEmployeeResponse).StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact]
    public void DeleteEmployee_ShouldProcessRequestAndInvokeUseCase()
    {
        // Arrange
        var employeeId = Guid.NewGuid();

        // Act
        var deleteEmployeeResponse = _employeesController.DeleteEmployee(employeeId);

        // Assert
        _deleteEmployeeUseCaseMock.Verify(mock => mock.Execute(employeeId), Times.Once);
        ((StatusCodeResult)deleteEmployeeResponse).StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}