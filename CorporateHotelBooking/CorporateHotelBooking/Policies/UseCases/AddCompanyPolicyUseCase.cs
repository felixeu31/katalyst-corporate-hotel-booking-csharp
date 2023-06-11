using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Application.Policies.UseCases;

namespace CorporateHotelBooking.Application.Policies.UseCases;

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
