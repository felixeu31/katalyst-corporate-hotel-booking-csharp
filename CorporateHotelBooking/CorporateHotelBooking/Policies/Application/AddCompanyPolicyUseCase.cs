using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Policies.Domain;

namespace CorporateHotelBooking.Policies.Application;

public class AddCompanyPolicyUseCase : IAddCompanyPolicyUseCase
{
    private readonly IPoliciesRepository _policiesRepository;

    public AddCompanyPolicyUseCase(IPoliciesRepository policiesRepository)
    {
        _policiesRepository = policiesRepository;
    }

    public void Execute(Guid companyId, List<string> roomTypes)
    {
        if (_policiesRepository.GetCompanyPolicy(CompanyId.From(companyId)) != default)
        {
            _policiesRepository.UpdateCompanyPolicy(new CompanyPolicy(CompanyId.From(companyId), roomTypes));
        }

        _policiesRepository.AddCompanyPolicy(new CompanyPolicy(CompanyId.From(companyId), roomTypes));
    }
}