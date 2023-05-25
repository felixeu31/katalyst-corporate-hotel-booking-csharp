namespace CorporateHotelBooking.Employees.Domain;

public class Employee
{
    private readonly int _companyId;
    private readonly int _employeeId;

    public Employee(int companyId, int employeeId)
    {
        _companyId = companyId;
        _employeeId = employeeId;
    }

    public int EmployeeId => _employeeId;
    public int CompanyId => _companyId;
}