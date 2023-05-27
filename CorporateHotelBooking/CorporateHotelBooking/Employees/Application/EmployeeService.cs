using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Employees.Application;

public class EmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public void AddEmployee(int companyId, Guid employeeId)
    {
        _employeeRepository.Add(new Employee(companyId, EmployeeId.From(employeeId)));
    }
}