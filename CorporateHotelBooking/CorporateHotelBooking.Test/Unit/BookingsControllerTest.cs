using CorporateHotelBooking.Api.Controllers;
using CorporateHotelBooking.Bookings.Application;
using CorporateHotelBooking.Hotels.Application;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace CorporateHotelBooking.Test.Unit;

public class BookingsControllerTest
{
    private Mock<IBookUseCase> _bookUseCaseMock;
    private BookingsController _bookingsController;

    public BookingsControllerTest()
    {
        _bookUseCaseMock = new Mock<IBookUseCase>();
        _bookingsController = new BookingsController(_bookUseCaseMock.Object);
    }
    [Fact]
    public void ShouldBookRoom()
    {
        // Arrange
        var bookingData = new BookingBody(1, Guid.NewGuid(), Guid.NewGuid(), "Deluxe", DateTime.Today, DateTime.Today.AddDays(1));

        // Act
        var addBookingResponse = _bookingsController.Book(bookingData);

        // Assert
        _bookUseCaseMock.Verify(mock => mock.Execute(
            bookingData.RoomNumber, 
            bookingData.HotelId, 
            bookingData.EmployeeId, 
            bookingData.RoomType, 
            bookingData.CheckIn, 
            bookingData.CheckOut), Times.Once);
        ((CreatedResult)addBookingResponse).StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

}