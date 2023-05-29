using CorporateHotelBooking.Bookings.Domain;

namespace CorporateHotelBooking.Bookings.Application;

public interface IBookUseCase
{
    Booking Execute(int roomNumber, Guid hotelId, Guid employeeId, string roomType, DateTime checkIn,
        DateTime checkOut);
}