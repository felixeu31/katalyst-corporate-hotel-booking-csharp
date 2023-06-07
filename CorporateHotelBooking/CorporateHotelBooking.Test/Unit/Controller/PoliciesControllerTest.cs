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
    private readonly Mock<IAddCompanyPolicyUseCase> _addCompanyPolicyUseCase;
    private readonly PoliciesController _policiesController;

    public PoliciesControllerTest()
    {
        _addEmployeePolicyUseCase = new Mock<IAddEmployeePolicyUseCase>();
        _addCompanyPolicyUseCase = new Mock<IAddCompanyPolicyUseCase>();
        _policiesController = new PoliciesController(_addEmployeePolicyUseCase.Object, _addCompanyPolicyUseCase.Object);
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

    [Fact]
    public void ShouldAddCompanyPolicy()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var policies = new List<string> { "Standard" };

        // Act
        var addHotelResponse = _policiesController.AddCompanyPolicy(new AddCompanyPolicyBody(companyId, policies));

        // Assert
        _addCompanyPolicyUseCase.Verify(mock => mock.Execute(companyId, policies), Times.Once);
        ((StatusCodeResult)addHotelResponse).StatusCode.Should().Be((int)HttpStatusCode.Created);
    }
}