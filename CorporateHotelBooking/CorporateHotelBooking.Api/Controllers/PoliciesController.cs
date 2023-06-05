using System.Net;
using CorporateHotelBooking.Policies.Application;
using Microsoft.AspNetCore.Mvc;

namespace CorporateHotelBooking.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class PoliciesController : ControllerBase
{
    private readonly IAddEmployeePolicyUseCase _addEmployeePolicyUseCase;

    public PoliciesController(IAddEmployeePolicyUseCase addEmployeePolicyUseCase)
    {
        _addEmployeePolicyUseCase = addEmployeePolicyUseCase;
    }

    [HttpPost("employee")]
    public StatusCodeResult AddEmployeePolicy(AddEmployeePolicyBody addEmployeePolicyBody)
    {
        _addEmployeePolicyUseCase.Execute(addEmployeePolicyBody.EmployeeId, addEmployeePolicyBody.Policies);

        return new StatusCodeResult((int)HttpStatusCode.Created);
    }
}