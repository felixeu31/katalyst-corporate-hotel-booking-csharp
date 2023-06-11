using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Bookings.Domain;

public interface IBookingRepository
{
    void Add(Booking booking);
    Booking? Get(BookingId bookingId);
    IEnumerable<Booking> GetBookingsBy(HotelId hotelId);
}