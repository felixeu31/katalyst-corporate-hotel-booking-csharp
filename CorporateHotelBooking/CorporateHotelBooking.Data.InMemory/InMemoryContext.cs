using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Policies.Domain;

namespace CorporateHotelBooking.Data.InMemory
{
    public class InMemoryContext
    {
        public InMemoryContext()
        {
            Bookings = new Dictionary<BookingId, Booking>();
            Employees = new Dictionary<EmployeeId, Employee>();
            Hotels = new Dictionary<HotelId, Hotel>();
            EmployeePolicies = new Dictionary<EmployeeId, EmployeePolicy>();
            CompanyPolicies = new Dictionary<CompanyId, CompanyPolicy>();
        }

        public readonly Dictionary<BookingId, Booking> Bookings;
        public readonly Dictionary<EmployeeId, Employee> Employees;
        public readonly Dictionary<HotelId, Hotel> Hotels;
        public readonly Dictionary<EmployeeId, EmployeePolicy> EmployeePolicies;
        public readonly Dictionary<CompanyId, CompanyPolicy> CompanyPolicies;
    }
}
