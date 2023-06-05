using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Hotels.Domain;
using CorporateHotelBooking.Policies.Domain;

namespace CorporateHotelBooking.Data
{
    public class InMemoryContext
    {
        public InMemoryContext()
        {
            Bookings = new Dictionary<BookingId, Booking>();
            Employees = new Dictionary<EmployeeId, Employee>();
            Hotels = new Dictionary<HotelId, Hotel>();
            EmployeePolicies = new Dictionary<EmployeeId, EmployeePolicy>();
        }

        public readonly Dictionary<BookingId, Booking> Bookings;
        public readonly Dictionary<EmployeeId, Employee> Employees;
        public readonly Dictionary<HotelId, Hotel> Hotels;
        public readonly Dictionary<EmployeeId, EmployeePolicy> EmployeePolicies;
    }
}
