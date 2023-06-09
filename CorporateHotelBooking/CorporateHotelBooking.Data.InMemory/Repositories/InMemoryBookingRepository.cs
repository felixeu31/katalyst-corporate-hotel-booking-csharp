using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;

namespace CorporateHotelBooking.Data.InMemory.Repositories;

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

    public IEnumerable<Booking> GetBookingsBy(HotelId hotelId)
    {
        return _context.Bookings.Select(x => x.Value).Where(x => x.HotelId.Equals(hotelId));
    }

    public void DeleteEmployeeBookings(EmployeeId employeeId)
    {
        foreach (var employeeBooking in _context.Bookings.Values.Where(x => x.BookedBy.Equals(employeeId)).ToList())
        {
            _context.Bookings.Remove(employeeBooking.BookingId);
        }
    }
}