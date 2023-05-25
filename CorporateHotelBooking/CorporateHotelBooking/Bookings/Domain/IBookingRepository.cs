using CorporateHotelBooking.Bookings.Domain;

namespace CorporateHotelBooking.Bookings.Domain;

public interface IBookingRepository
{
    void Add(Booking booking);
    Booking? Get(Guid bookingId);
}