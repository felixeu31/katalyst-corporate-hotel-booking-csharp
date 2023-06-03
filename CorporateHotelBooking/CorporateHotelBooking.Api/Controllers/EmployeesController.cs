using CorporateHotelBooking.Hotels.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using CorporateHotelBooking.Employees.Application;

namespace CorporateHotelBooking.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IAddEmployeeUseCase _addEmployeeUseCase;

        public EmployeesController(IAddEmployeeUseCase addEmployeeUseCase)
        {
            _addEmployeeUseCase = addEmployeeUseCase;
        }

        [HttpPost]
        public IActionResult AddHotel(AddEmployeeData employeeData)
        {
            _addEmployeeUseCase.Execute(employeeData.CompanyId, employeeData.EmployeeId);

            return new StatusCodeResult((int)HttpStatusCode.Created);
        }
    }

    public record AddEmployeeData(Guid CompanyId, Guid EmployeeId) { }
}
