using CorporateHotelBooking.Bookings.Application;
using CorporateHotelBooking.Hotels.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorporateHotelBooking.Bookings.Domain;

namespace CorporateHotelBooking.Test.Unit
{
    public class BookingServiceUnitTest
    {
        private readonly Mock<IBookingRepository> _bookingRepository;

        public BookingServiceUnitTest()
        {
            _bookingRepository = new();
        }

        [Fact]
        public void AddBooking_ShouldStoreBooking()
        {
            // Arrange
            var bookingService = new BookingService(_bookingRepository.Object);
            var hotelId = Guid.NewGuid();
            var roomNumber = 1;
            var employeeId = Guid.NewGuid();
            var roomType = "Deluxe";
            var checkIn = DateTime.Today.AddDays(1);
            var chekout = DateTime.Today.AddDays(2);

            // Act
            bookingService.Book(roomNumber, hotelId, employeeId, roomType, checkIn, chekout);

            // Assert
            _bookingRepository.Verify(x => x.Add(It.IsAny<Booking>()), Times.Once());

        }
    }
}
