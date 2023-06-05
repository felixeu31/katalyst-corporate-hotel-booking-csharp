using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Policies.Domain;

public interface IPoliciesRepository
{
    void AddEmployeePolicy(EmployeePolicy employeePolicy);
    EmployeePolicy? GetEmployeePolicy(EmployeeId employeeId);
}