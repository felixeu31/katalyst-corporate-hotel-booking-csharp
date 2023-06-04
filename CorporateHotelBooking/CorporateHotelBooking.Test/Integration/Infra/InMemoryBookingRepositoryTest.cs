using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Bookings.Infra;
using CorporateHotelBooking.Data;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Hotels.Domain;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Integration.Infra
{
    public class InMemoryBookingRepositoryTest
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly InMemoryContext _inMemoryContext;

        public InMemoryBookingRepositoryTest()
        {
            _inMemoryContext = new InMemoryContext();
            _bookingRepository = new InMemoryBookingRepository(_inMemoryContext);
        }

        [Fact]
        public void should_add_booking()
        {
            // Arrange
            HotelId hotelId = HotelId.New();
            int roomNumber = 1;
            EmployeeId employeeId = EmployeeId.New();
            string roomType = "Deluxe";
            DateTime checkIn = DateTime.Today.AddDays(1);
            DateTime chekout = DateTime.Today.AddDays(2);
            var newBooking = new Booking(roomNumber, hotelId, employeeId, roomType, checkIn, chekout);

            // Act
            _bookingRepository.Add(newBooking);

            // Assert
            Booking? booking = _inMemoryContext.Bookings[newBooking.BookingId];
            booking.Should().NotBeNull();
            booking.RoomNumber.Should().Be(1);
            booking.HotelId.Should().Be(hotelId);
            booking.RoomType.Should().Be("Deluxe");
            booking.CheckIn.Should().BeSameDateAs(DateTime.Today.AddDays(1));
            booking.CheckOut.Should().BeSameDateAs(DateTime.Today.AddDays(2));
        }

        [Fact]
        public void should_retrieve_booking()
        {
            // Arrange
            HotelId hotelId = HotelId.New();
            int roomNumber = 1;
            EmployeeId employeeId = EmployeeId.New();
            string roomType = "Deluxe";
            DateTime checkIn = DateTime.Today.AddDays(1);
            DateTime chekout = DateTime.Today.AddDays(2);
            var newBooking = new Booking(roomNumber, hotelId, employeeId, roomType, checkIn, chekout);
            _inMemoryContext.Bookings.Add(newBooking.BookingId, newBooking);

            // Act
            Booking? booking = _bookingRepository.Get(newBooking.BookingId);

            // Assert
            booking.Should().NotBeNull();
            booking.RoomNumber.Should().Be(1);
            booking.HotelId.Should().Be(hotelId);
            booking.RoomType.Should().Be("Deluxe");
            booking.CheckIn.Should().BeSameDateAs(DateTime.Today.AddDays(1));
            booking.CheckOut.Should().BeSameDateAs(DateTime.Today.AddDays(2));
        }

    }
}
