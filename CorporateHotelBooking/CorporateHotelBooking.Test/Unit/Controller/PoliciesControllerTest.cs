using CorporateHotelBooking.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using CorporateHotelBooking.Policies.Application;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Unit.Controller;

public class PoliciesControllerTest
{
    private readonly Mock<IAddEmployeePolicyUseCase> _addEmployeePolicyUseCase;
    private readonly PoliciesController _policiesController;

    public PoliciesControllerTest()
    {
        _addEmployeePolicyUseCase = new Mock<IAddEmployeePolicyUseCase>();
        _policiesController = new PoliciesController(_addEmployeePolicyUseCase.Object);
    }

    [Fact]
    public void ShouldAddEmployeePolicy()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var policies = new List<string> { "Standard" };

        // Act
        var addHotelResponse = _policiesController.AddEmployeePolicy(new AddEmployeePolicyBody(employeeId, policies));

        // Assert
        _addEmployeePolicyUseCase.Verify(mock => mock.Execute(employeeId, policies), Times.Once);
        ((StatusCodeResult)addHotelResponse).StatusCode.Should().Be((int)HttpStatusCode.Created);
    }
}