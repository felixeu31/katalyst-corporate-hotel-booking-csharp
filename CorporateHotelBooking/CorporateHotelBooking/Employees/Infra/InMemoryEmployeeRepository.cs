using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Employees.Infra;

public class InMemoryEmployeeRepository : IEmployeeRepository
{
    private readonly Dictionary<EmployeeId, Employee> _employees;

    public InMemoryEmployeeRepository()
    {
        _employees = new Dictionary<EmployeeId, Employee>();
    }

    public void Add(Employee employee)
    {
        _employees.Add(employee.EmployeeId, employee);
    }

    public Employee? Get(EmployeeId employeeId)
    {

        if (_employees.TryGetValue(employeeId, out var employee))
        {
            return employee;
        }
        return null;
    }
}