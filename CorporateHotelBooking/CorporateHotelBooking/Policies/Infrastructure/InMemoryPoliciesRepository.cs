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

    public EmployeePolicy? GetEmployeePolicy(EmployeeId employeeId)
    {
        return _context.EmployeePolicies.TryGetValue(employeeId, out var employeePolicy) ? employeePolicy : null;
    }

    public void AddCompanyPolicy(CompanyPolicy companyPolicy)
    {
        _context.CompanyPolicies.Add(companyPolicy.CompanyId, companyPolicy);
    }

    public CompanyPolicy? GetCompanyPolicy(CompanyId companyId)
    {
        return _context.CompanyPolicies.TryGetValue(companyId, out var companyPolicy) ? companyPolicy : null;
    }

    public void UpdateCompanyPolicy(CompanyPolicy companyPolicy)
    {
        _context.CompanyPolicies[companyPolicy.CompanyId] = companyPolicy;
    }

    public void UpdateEmployeePolicy(EmployeePolicy employeePolicy)
    {
        _context.EmployeePolicies[employeePolicy.EmployeeId] = employeePolicy;
    }
}