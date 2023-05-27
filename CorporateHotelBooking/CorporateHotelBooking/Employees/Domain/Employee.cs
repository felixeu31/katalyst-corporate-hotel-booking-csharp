namespace CorporateHotelBooking.Employees.Domain;

public class Employee
{
    private readonly int _companyId;
    private readonly EmployeeId _employeeId;

    public Employee(int companyId, EmployeeId employeeId)
    {
        _companyId = companyId;
        _employeeId = employeeId;
    }

    public EmployeeId EmployeeId => _employeeId;
    public int CompanyId => _companyId;
}