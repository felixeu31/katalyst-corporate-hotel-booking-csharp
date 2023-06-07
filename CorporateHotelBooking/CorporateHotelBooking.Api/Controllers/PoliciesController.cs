using System.Net;
using CorporateHotelBooking.Policies.Application;
using Microsoft.AspNetCore.Mvc;

namespace CorporateHotelBooking.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class PoliciesController : ControllerBase
{
    private readonly IAddEmployeePolicyUseCase _addEmployeePolicyUseCase;
    private readonly IAddCompanyPolicyUseCase _addCompanyPolicyUseCase;

    public PoliciesController(IAddEmployeePolicyUseCase addEmployeePolicyUseCase,
        IAddCompanyPolicyUseCase addCompanyPolicyUseCase)
    {
        _addEmployeePolicyUseCase = addEmployeePolicyUseCase;
        _addCompanyPolicyUseCase = addCompanyPolicyUseCase;
    }

    [HttpPost("employee")]
    public StatusCodeResult AddEmployeePolicy(AddEmployeePolicyBody addEmployeePolicyBody)
    {
        _addEmployeePolicyUseCase.Execute(addEmployeePolicyBody.EmployeeId, addEmployeePolicyBody.RoomTypes);

        return new StatusCodeResult((int)HttpStatusCode.Created);
    }

    [HttpPost("company")]
    public StatusCodeResult AddCompanyPolicy(AddCompanyPolicyBody addCompanyPolicyBody)
    {
        _addCompanyPolicyUseCase.Execute(addCompanyPolicyBody.CompanyId, addCompanyPolicyBody.RoomTypes);

        return new StatusCodeResult((int)HttpStatusCode.Created);
    }
}

public record AddCompanyPolicyBody(Guid CompanyId, List<string> RoomTypes);

public record AddEmployeePolicyBody(Guid EmployeeId, List<string> RoomTypes);