using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Employees.Application;

public class EmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public void AddEmployee(int companyId, int employeeId)
    {
        _employeeRepository.Add(new Employee(companyId, employeeId));
    }
}