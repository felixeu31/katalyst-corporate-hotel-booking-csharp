namespace CorporateHotelBooking.Application.Policies.UseCases;

public interface IAddCompanyPolicyUseCase
{
    void Execute(Guid companyId, List<string> roomTypes);
}
