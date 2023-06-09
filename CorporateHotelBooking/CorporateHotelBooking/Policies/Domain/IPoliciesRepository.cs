using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Policies.Domain;

public interface IPoliciesRepository
{
    void AddEmployeePolicy(EmployeePolicy employeePolicy);
    EmployeePolicy? GetEmployeePolicy(EmployeeId employeeId);
    void AddCompanyPolicy(CompanyPolicy companyPolicy);
    CompanyPolicy? GetCompanyPolicy(CompanyId from);
    void UpdateCompanyPolicy(CompanyPolicy companyPolicy);
    void UpdateEmployeePolicy(EmployeePolicy isAny);
}