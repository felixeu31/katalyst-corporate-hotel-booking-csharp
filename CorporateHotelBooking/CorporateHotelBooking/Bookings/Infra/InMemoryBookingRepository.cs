using CorporateHotelBooking.Bookings.Domain;

namespace CorporateHotelBooking.Bookings.Infra;

public class InMemoryBookingRepository : IBookingRepository
{
    private readonly Dictionary<Guid, Booking> _bookings;

    public InMemoryBookingRepository()
    {
        _bookings = new Dictionary<Guid, Booking>();
    }

    public void Add(Booking booking)
    {
        _bookings.Add(booking.BookingId, booking);
    }

    public Booking? Get(Guid bookingId)
    {
        if (_bookings.TryGetValue(bookingId, out var booking))
        {
            return booking;
        }
        return null;
    }
}