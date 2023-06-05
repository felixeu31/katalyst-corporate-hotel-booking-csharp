using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Policies.Domain;

public class EmployeePolicy
{
    public EmployeeId EmployeeId { get; }
    public List<string> Policies { get; }

    public EmployeePolicy(EmployeeId employeeId, List<string> policies)
    {
        EmployeeId = employeeId;
        Policies = policies;
    }
}