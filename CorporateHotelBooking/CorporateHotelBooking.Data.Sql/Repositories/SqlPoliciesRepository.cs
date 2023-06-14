using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Policies.Domain;

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
        throw new NotImplementedException();
    }

    public EmployeePolicy? GetEmployeePolicy(EmployeeId employeeId)
    {
        throw new NotImplementedException();
    }

    public void AddCompanyPolicy(CompanyPolicy companyPolicy)
    {
        throw new NotImplementedException();
    }

    public CompanyPolicy? GetCompanyPolicy(CompanyId from)
    {
        throw new NotImplementedException();
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