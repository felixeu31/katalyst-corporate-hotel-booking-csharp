using CorporateHotelBooking.Policies.Application;
using Microsoft.AspNetCore.Mvc;

namespace CorporateHotelBooking.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class PoliciesController : ControllerBase
{
    public PoliciesController(IAddEmployeePolicyUseCase addEmployeePolicyUseCase)
    {
        throw new NotImplementedException();
    }

    [HttpPost("employee")]
    public StatusCodeResult AddEmployeePolicy(AddEmployeePolicyBody addEmployeePolicyBody)
    {
        throw new NotImplementedException();
    }
}