using CorporateHotelBooking.Application.Bookings.Domain.Exceptions;
using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Test.TestUtilities;
using FluentAssertions;
using CorporateHotelBooking.Test.Constants;

namespace CorporateHotelBooking.Test.Unit.Domain;

[Trait(TestTrait.Category, TestCategory.Unit)]
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