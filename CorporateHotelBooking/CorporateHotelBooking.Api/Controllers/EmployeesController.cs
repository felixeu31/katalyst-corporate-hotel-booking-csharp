using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using CorporateHotelBooking.Application.Employees.UseCases;

namespace CorporateHotelBooking.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IAddEmployeeUseCase _addEmployeeUseCase;
        private readonly IDeleteEmployeeUseCase _deleteEmployeeUseCase;

        public EmployeesController(IAddEmployeeUseCase addEmployeeUseCase, IDeleteEmployeeUseCase deleteEmployeeUseCase)
        {
            _addEmployeeUseCase = addEmployeeUseCase;
            _deleteEmployeeUseCase = deleteEmployeeUseCase;
        }

        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeData employeeData)
        {
            _addEmployeeUseCase.Execute(employeeData.CompanyId, employeeData.EmployeeId);

            return new StatusCodeResult((int)HttpStatusCode.Created);
        }

        [HttpDelete("{employeeId}")]
        public IActionResult DeleteEmployee(Guid employeeId)
        {
            _deleteEmployeeUseCase.Execute(employeeId);

            return new StatusCodeResult((int)HttpStatusCode.OK);
        }
    }

    public record AddEmployeeData(Guid CompanyId, Guid EmployeeId) { }
}
