namespace CorporateHotelBooking.Application.Policies.UseCases;

public interface IIsBookingAllowedUseCase
{
    bool Execute(Guid employeeId, string roomType);
}
