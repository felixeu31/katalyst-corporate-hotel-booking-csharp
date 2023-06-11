using CorporateHotelBooking.Application.Employees.Domain;

namespace CorporateHotelBooking.Application.Employees.Domain;

public interface IEmployeeRepository
{
    void Add(Employee employee);
    Employee? Get(EmployeeId employeeId);
    void Delete(EmployeeId employeeId);
}
