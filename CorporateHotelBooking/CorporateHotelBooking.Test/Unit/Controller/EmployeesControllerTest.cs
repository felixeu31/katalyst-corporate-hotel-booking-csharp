using CorporateHotelBooking.Api.Controllers;
using CorporateHotelBooking.Employees.Application;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Unit.Controller
{
    public class EmployeesControllerTest
    {
        [Fact]
        public void AddEmployee_ShouldProcessRequestAndInvokeUseCase()
        {
            // Arrange
            var addEmployeeUseCaseMock = new Mock<IAddEmployeeUseCase>();
            var employeesController = new EmployeesController(addEmployeeUseCaseMock.Object);
            var addEmployeeData = new AddEmployeeData(Guid.NewGuid(), Guid.NewGuid());

            // Act
            var addEmployeeResponse = employeesController.AddHotel(addEmployeeData);

            // Assert
            addEmployeeUseCaseMock.Verify(mock => mock.Execute(addEmployeeData.CompanyId, addEmployeeData.EmployeeId), Times.Once);
            ((StatusCodeResult)addEmployeeResponse).StatusCode.Should().Be((int)HttpStatusCode.Created);
        }
    }
}
