using CorporateHotelBooking.Data;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Hotels.Domain;
using CorporateHotelBooking.Policies.Domain;

namespace CorporateHotelBooking.Policies.Infrastructure;

public class InMemoryPoliciesRepository : IPoliciesRepository
{
    private readonly InMemoryContext _context;

    public InMemoryPoliciesRepository(InMemoryContext context)
    {
        _context = context;
    }

    public void AddEmployeePolicy(EmployeePolicy employeePolicy)
    {
        _context.EmployeePolicies.Add(employeePolicy.EmployeeId, employeePolicy);
    }
}