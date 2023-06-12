using CorporateHotelBooking.Test.Constants;
using FluentAssertions;
using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Data.InMemory;
using CorporateHotelBooking.Data.InMemory.Repositories;

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
            string roomType = SampleData.RoomTypes.Deluxe;
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
            booking.RoomType.Should().Be(SampleData.RoomTypes.Deluxe);
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
            string roomType = SampleData.RoomTypes.Deluxe;
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
            booking.RoomType.Should().Be(SampleData.RoomTypes.Deluxe);
            booking.CheckIn.Should().BeSameDateAs(DateTime.Today.AddDays(1));
            booking.CheckOut.Should().BeSameDateAs(DateTime.Today.AddDays(2));
        }

        [Fact]
        public void should_retrieve_bookings_by_hotel()
        {
            // Arrange
            HotelId hotelId = HotelId.New();
            HotelId anotherHotelId = HotelId.New();
            EmployeeId employeeId = EmployeeId.New();
            var booking1 = new Booking(1, hotelId, employeeId, SampleData.RoomTypes.Deluxe, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            var booking2 = new Booking(2, hotelId, employeeId, SampleData.RoomTypes.Deluxe, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            var booking3 = new Booking(2, anotherHotelId, employeeId, SampleData.RoomTypes.Deluxe, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            _inMemoryContext.Bookings.Add(booking1.BookingId, booking1);
            _inMemoryContext.Bookings.Add(booking2.BookingId, booking2);
            _inMemoryContext.Bookings.Add(booking3.BookingId, booking3);

            // Act
            var bookings = _bookingRepository.GetBookingsBy(hotelId);

            // Assert
            bookings.Should().NotBeNull();
            bookings.Should().HaveCount(2);
            bookings.Should().AllSatisfy(x => x.HotelId.Equals(hotelId));
        }

        [Fact]
        public void should_remove_bookings_by_employee()
        {
            // Arrange
            HotelId hotelId = HotelId.New();
            EmployeeId employeeId = EmployeeId.New();
            var booking1 = new Booking(1, hotelId, employeeId, SampleData.RoomTypes.Deluxe, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            var booking2 = new Booking(2, hotelId, employeeId, SampleData.RoomTypes.Deluxe, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            var booking3 = new Booking(2, hotelId, employeeId, SampleData.RoomTypes.Deluxe, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            _inMemoryContext.Bookings.Add(booking1.BookingId, booking1);
            _inMemoryContext.Bookings.Add(booking2.BookingId, booking2);
            _inMemoryContext.Bookings.Add(booking3.BookingId, booking3);

            // Act
            _bookingRepository.DeleteEmployeeBookings(employeeId);

            // Assert
            var employeeBookings =
                _inMemoryContext.Bookings.Select(x => x.Value).Where(x => x.BookedBy.Equals(employeeId));
            employeeBookings.Should().HaveCount(0);
        }

    }
}
