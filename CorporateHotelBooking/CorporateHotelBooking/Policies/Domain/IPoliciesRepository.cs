using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Policies.Domain;

namespace CorporateHotelBooking.Application.Policies.Domain;

public interface IPoliciesRepository
{
    void AddEmployeePolicy(EmployeePolicy employeePolicy);
    EmployeePolicy? GetEmployeePolicy(EmployeeId employeeId);
    void AddCompanyPolicy(CompanyPolicy companyPolicy);
    CompanyPolicy? GetCompanyPolicy(CompanyId companyId);
    void UpdateCompanyPolicy(CompanyPolicy companyPolicy);
    void UpdateEmployeePolicy(EmployeePolicy employeePolicy);
    void DeleteEmployeePolicies(EmployeeId employeeId);
}
