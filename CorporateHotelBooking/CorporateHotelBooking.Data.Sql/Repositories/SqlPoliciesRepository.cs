using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Data.Sql.Mappers;

namespace CorporateHotelBooking.Data.Sql.Repositories;

public class SqlPoliciesRepository : IPoliciesRepository
{
    private readonly CorporateHotelDbContext _context;

    public SqlPoliciesRepository(CorporateHotelDbContext context)
    {
        _context = context;
    }


    public void AddEmployeePolicy(EmployeePolicy employeePolicy)
    {
        var employeePolicyData = EmployeePolicyDataMapper.MapEmployeePolicyDataFrom(employeePolicy);

        _context.EmployeePolicies.Add(employeePolicyData);

        _context.SaveChanges();
    }

    public EmployeePolicy? GetEmployeePolicy(EmployeeId employeeId)
    {
        var employeePolicyData = _context.EmployeePolicies.FirstOrDefault(x => x.EmployeeId == employeeId.Value);

        var employeePolicy = EmployeePolicyDataMapper.HydrateDomainFrom(employeePolicyData);

        return employeePolicy;
    }

    public void AddCompanyPolicy(CompanyPolicy companyPolicy)
    {
        var companyPolicyData = CompanyPolicyDataMapper.MapCompanyPolicyDataFrom(companyPolicy);

        _context.CompanyPolicies.Add(companyPolicyData);

        _context.SaveChanges();
    }

    public CompanyPolicy? GetCompanyPolicy(CompanyId companyId)
    {
        var companyPolicyData = _context.CompanyPolicies.FirstOrDefault(x => x.CompanyId == companyId.Value);

        var companyPolicy = CompanyPolicyDataMapper.HydrateDomainFrom(companyPolicyData);

        return companyPolicy;
    }

    public void UpdateCompanyPolicy(CompanyPolicy companyPolicy)
    {
        throw new NotImplementedException();
    }

    public void UpdateEmployeePolicy(EmployeePolicy isAny)
    {
        throw new NotImplementedException();
    }

    public void DeleteEmployeePolicies(EmployeeId employeeId)
    {
        throw new NotImplementedException();
    }
}