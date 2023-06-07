namespace CorporateHotelBooking.Policies.Application;

public interface IAddCompanyPolicyUseCase
{
    void Execute(Guid companyId, List<string> roomTypes);
}