using CorporateHotelBooking.Bookings.Application;
using CorporateHotelBooking.Hotels.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Policies.Application;
using CorporateHotelBooking.Hotels.Application;
using FluentAssertions;
using CorporateHotelBooking.Test.Unit.Controller;
using CorporateHotelBooking.Bookings.Domain.Exceptions;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Test.Constants;

namespace CorporateHotelBooking.Test.Unit.UseCases
{
    public class BookUseCaseTest
    {
        private readonly Mock<IBookingRepository> _bookingRepository;
        private readonly Mock<IIsBookingAllowedUseCase> _isBookingAllowedUseCase;
        private readonly Mock<IHotelRepository> _hotelRepository;
        private BookUseCase _bookUseCase;

        public BookUseCaseTest()
        {
            _bookingRepository = new();
            _isBookingAllowedUseCase = new();
            _hotelRepository = new();
            var hotel = new Hotel(HotelId.New(), "Westing");
            hotel.SetRoom(1, RoomTypes.Deluxe);
            _hotelRepository.Setup(x => x.Get(It.IsAny<HotelId>())).Returns(hotel);
            _bookUseCase = new BookUseCase(_bookingRepository.Object, _isBookingAllowedUseCase.Object, _hotelRepository.Object);
        }

        [Fact]
        public void AddBooking_ShouldStoreBooking()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();
            var roomType = RoomTypes.Deluxe;
            var checkIn = DateTime.Today.AddDays(1);
            var chekout = DateTime.Today.AddDays(2);
            _isBookingAllowedUseCase.Setup(x => x.Execute(employeeId, roomType)).Returns(true);

            // Act
            _bookUseCase.Execute(hotelId, employeeId, roomType, checkIn, chekout);

            // Assert
            _bookingRepository.Verify(x => x.Add(It.IsAny<Booking>()), Times.Once());

        }

        [Fact]
        public void AddBooking_WhenPolicyIsNotContained_ShouldThrowEmployeePolicyException()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();
            var roomType = RoomTypes.Deluxe;
            var checkIn = DateTime.Today.AddDays(1);
            var chekout = DateTime.Today.AddDays(2);
            _isBookingAllowedUseCase.Setup(x => x.Execute(employeeId, roomType)).Returns(false);
            
            // Act
            Action action = () => _bookUseCase.Execute(hotelId, employeeId, roomType, checkIn, chekout);

            // Assert
            action.Should().Throw<EmployeeBookingPolicyException>();
        }

    }
}
