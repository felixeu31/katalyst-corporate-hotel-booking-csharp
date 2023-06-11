using CorporateHotelBooking.Application.Employees.Domain;

namespace CorporateHotelBooking.Application.Employees.Domain;

public class Employee
{
    private readonly CompanyId _companyId;
    private readonly EmployeeId _employeeId;

    public Employee(CompanyId companyId, EmployeeId employeeId)
    {
        _companyId = companyId;
        _employeeId = employeeId;
    }

    public EmployeeId EmployeeId => _employeeId;
    public CompanyId CompanyId => _companyId;
}
