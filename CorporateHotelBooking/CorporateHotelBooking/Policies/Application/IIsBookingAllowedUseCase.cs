namespace CorporateHotelBooking.Policies.Application;

public interface IIsBookingAllowedUseCase
{
    bool Execute(Guid employeeId, string roomType);
}