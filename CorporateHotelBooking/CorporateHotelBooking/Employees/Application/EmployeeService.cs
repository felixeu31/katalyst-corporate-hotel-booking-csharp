using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Employees.Application;

public class EmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public void AddEmployee(Guid companyId, Guid employeeId)
    {
        _employeeRepository.Add(new Employee(CompanyId.From(companyId), EmployeeId.From(employeeId)));
    }
}