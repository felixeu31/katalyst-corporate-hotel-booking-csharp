using CorporateHotelBooking.Data;
using CorporateHotelBooking.Data.InMemory;
using CorporateHotelBooking.Application.Employees.Domain;

namespace CorporateHotelBooking.Data.InMemory.Repositories;

public class InMemoryEmployeeRepository : IEmployeeRepository
{
    private readonly InMemoryContext _context;

    public InMemoryEmployeeRepository(InMemoryContext context)
    {
        _context = context;
    }

    public void Add(Employee employee)
    {
        _context.Employees.Add(employee.EmployeeId, employee);
    }

    public Employee? Get(EmployeeId employeeId)
    {
        return _context.Employees.TryGetValue(employeeId, out var employee) ? employee : null;
    }

    public void Delete(EmployeeId employeeId)
    {
        _context.Employees.Remove(employeeId);
    }
}
