using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Policies.Domain;

public interface IPoliciesRepository
{
    void AddEmployeePolicy(EmployeeId employeeId, List<string> policies);
}