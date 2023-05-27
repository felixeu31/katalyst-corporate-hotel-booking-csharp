namespace CorporateHotelBooking.Employees.Domain;

public interface IEmployeeRepository
{
    void Add(Employee employee);
    Employee? Get(EmployeeId employeeId);
}