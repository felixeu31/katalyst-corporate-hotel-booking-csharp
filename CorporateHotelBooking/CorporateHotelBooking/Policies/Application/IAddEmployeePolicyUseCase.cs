namespace CorporateHotelBooking.Policies.Application;

public interface IAddEmployeePolicyUseCase
{
    void Execute(Guid employeeId, List<string> roomTypes);
}