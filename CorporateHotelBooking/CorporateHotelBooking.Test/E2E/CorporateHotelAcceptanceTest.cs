using CorporateHotelBooking.Bookings.Application;
using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Bookings.Infra;
using CorporateHotelBooking.Employees.Application;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Employees.Infra;
using CorporateHotelBooking.Hotels.Application;
using CorporateHotelBooking.Hotels.Domain;
using CorporateHotelBooking.Hotels.Infrastructure;
using FluentAssertions;

namespace CorporateHotelBooking.Test.E2E
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>dotnet watch test --project .\CorporateHotelBooking.Test\CorporateHotelBooking.Test.csproj</remarks>
    public class CorporateHotelAcceptanceTest
    {
        private readonly HotelService _hotelService;
        private readonly BookingService _bookingService;
        private readonly EmployeeService _employeeService;

        public CorporateHotelAcceptanceTest()
        {
            IHotelRepository hotelRepository = new InMemoryHotelRepository();
            IEmployeeRepository employeeRepository = new InMemoryEmployeeRepository();
            IBookingRepository bookingRepository = new InMemoryBookingRepository();
            _employeeService = new EmployeeService(employeeRepository);
            _bookingService = new BookingService(bookingRepository);
            _hotelService = new HotelService(hotelRepository);
        }

        [Fact]
        public async Task an_employee_should_be_able_to_book_a_room()
        {
            // Arrange
            var companyId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();
            var hotelId = Guid.NewGuid();
            var hotelName = "Wesing";
            var roomType = "Suite";
            var roomNumber = 1;
            var checkIn = DateTime.Today;
            var checkOut = DateTime.Today.AddDays(7);
            _hotelService.AddHotel(hotelId, hotelName);
            _hotelService.SetRoom(hotelId, roomNumber, roomType);
            _employeeService.AddEmployee(companyId, employeeId);

            // Act
            var booking = _bookingService.Book(roomNumber, hotelId, employeeId, roomType, checkIn, checkOut);

            // Assert
            booking.HotelId.Should().Be(HotelId.From(hotelId));
            booking.BookedBy.Should().Be(EmployeeId.From(employeeId));
            booking.RoomType.Should().Be(roomType);
            booking.RoomNumber.Should().Be(roomNumber);
            booking.CheckIn.Should().BeSameDateAs(checkIn);
            booking.CheckOut.Should().BeSameDateAs(checkOut);
        }

    }
}