using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Bookings.Infra;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Hotels.Domain;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Integration
{
    public class InMemoryBookingRepositoryTest
    {
        private readonly IBookingRepository _bookingRepository;

        public InMemoryBookingRepositoryTest()
        {
            _bookingRepository = new InMemoryBookingRepository();
        }

        [Fact]
        public void should_retrieve_added_booking()
        {
            // Arrange
            HotelId hotelId = HotelId.New();
            int roomNumber = 1;
            EmployeeId employeeId = EmployeeId.New();
            string roomType = "Deluxe";
            DateTime checkIn = DateTime.Today.AddDays(1);
            DateTime chekout = DateTime.Today.AddDays(2);

            // Act
            var newBooking = new Booking(roomNumber, hotelId, employeeId, roomType, checkIn, chekout);
            _bookingRepository.Add(newBooking);
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
