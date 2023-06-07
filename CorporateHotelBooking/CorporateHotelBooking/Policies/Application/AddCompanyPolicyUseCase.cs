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
        _policiesRepository.AddCompanyPolicy(new CompanyPolicy(CompanyId.From(companyId), roomTypes));
    }
}