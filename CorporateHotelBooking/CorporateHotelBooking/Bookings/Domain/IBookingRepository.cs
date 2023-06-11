using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;

namespace CorporateHotelBooking.Application.Bookings.Domain;

public interface IBookingRepository
{
    void Add(Booking booking);
    Booking? Get(BookingId bookingId);
    IEnumerable<Booking> GetBookingsBy(HotelId hotelId);
    void DeleteEmployeeBookings(EmployeeId employeeId);
}
