using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Data
{
    public class InMemoryContext
    {
        public InMemoryContext()
        {
            Bookings = new Dictionary<BookingId, Booking>();
            Employees = new Dictionary<EmployeeId, Employee>();
            Hotels = new Dictionary<HotelId, Hotel>();
        }

        public readonly Dictionary<BookingId, Booking> Bookings;
        public readonly Dictionary<EmployeeId, Employee> Employees;
        public readonly Dictionary<HotelId, Hotel> Hotels;
    }
}
