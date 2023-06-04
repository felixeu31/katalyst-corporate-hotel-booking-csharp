using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Data;

namespace CorporateHotelBooking.Bookings.Infra;

public class InMemoryBookingRepository : IBookingRepository
{
    private readonly InMemoryContext _context;

    public InMemoryBookingRepository(InMemoryContext context)
    {
        _context = context;
    }

    public void Add(Booking booking)
    {
        _context.Bookings.Add(booking.BookingId, booking);
    }

    public Booking? Get(BookingId bookingId)
    {
        return _context.Bookings.TryGetValue(bookingId, out var booking) ? booking : null;
    }
}