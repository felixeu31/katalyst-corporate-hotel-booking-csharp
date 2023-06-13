using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Data.Sql.Mappers;

namespace CorporateHotelBooking.Data.Sql.Repositories;

public class SqlEmployeeRepository : IEmployeeRepository
{
    private readonly CorporateHotelDbContext _context;

    public SqlEmployeeRepository(CorporateHotelDbContext context)
    {
        _context = context;
    }

    public void Add(Employee employee)
    {
        var employeeData = EmployeeDataMapper.MapEmployeeDataFrom(employee);

        _context.Employees.Add(employeeData);

        _context.SaveChanges();
    }

    public Employee? Get(EmployeeId employeeId)
    {
        throw new NotImplementedException();
    }

    public void Delete(EmployeeId employeeId)
    {
        var employeeData = _context.Employees.SingleOrDefault(x => x.EmployeeId == employeeId.Value);
        
        _context.Employees.Remove(employeeData);
        
        _context.SaveChanges();
    }
}