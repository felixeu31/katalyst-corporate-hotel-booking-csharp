using CorporateHotelBooking.Application.Bookings.Domain.Exceptions;
using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Test.Constants;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Unit.Domain;

public class BookingTest
{
    [Fact]
    public void NewBook_WhenBookingPeriodIsNotValid_ShouldThrowException()
    {
        // Arrange
        // Act
        Action action = () => new Booking(1, HotelId.New(), EmployeeId.New(), SampleData.RoomTypes.Deluxe, DateTime.Today.AddDays(1), DateTime.Today.AddDays(1));

        // Assert
        action.Should().Throw<InvalidBookingPeriodException>();
    }
}