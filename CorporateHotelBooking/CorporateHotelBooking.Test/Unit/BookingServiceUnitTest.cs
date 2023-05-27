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
            Guid hotelId = Guid.NewGuid();
            int roomNumber = 1;
            int employeeId = 1;
            string roomType = "Deluxe";
            DateTime checkIn = DateTime.Today.AddDays(1);
            DateTime chekout = DateTime.Today.AddDays(2);

            // Act
            bookingService.Book(roomNumber, hotelId, employeeId, roomType, checkIn, chekout);

            // Assert
            _bookingRepository.Verify(x => x.Add(It.IsAny<Booking>()), Times.Once());

        }
    }
}
