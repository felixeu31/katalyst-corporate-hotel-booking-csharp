using CorporateHotelBooking.Application.Bookings.Domain;

namespace CorporateHotelBooking.Application.Bookings.UseCases;

public interface IBookUseCase
{
    Booking Execute(Guid hotelId, Guid employeeId, string roomType, DateTime checkIn,
        DateTime checkOut);
}
