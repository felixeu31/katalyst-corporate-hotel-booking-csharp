namespace CorporateHotelBooking.Application.Policies.UseCases;

public interface IAddEmployeePolicyUseCase
{
    void Execute(Guid employeeId, List<string> roomTypes);
}
