using CorporateHotelBooking.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using CorporateHotelBooking.Test.Constants;
using CorporateHotelBooking.Application.Bookings.Domain.Exceptions;
using CorporateHotelBooking.Application.Employees.Domain.Exceptions;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Application.Bookings.UseCases;

namespace CorporateHotelBooking.Test.Unit.Controller;

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
        var bookingData = new BookingBody(Guid.NewGuid(), Guid.NewGuid(), RoomTypes.Deluxe, DateTime.Today, DateTime.Today.AddDays(1));

        // Act
        var addBookingResponse = _bookingsController.Book(bookingData);

        // Assert
        _bookUseCaseMock.Verify(mock => mock.Execute(
            bookingData.HotelId,
            bookingData.EmployeeId,
            bookingData.RoomType,
            bookingData.CheckIn,
            bookingData.CheckOut), Times.Once);
        ((CreatedResult)addBookingResponse).StatusCode.Should().Be((int)HttpStatusCode.Created);
    }


    [Fact]
    public void BookRoom_WhenEmployeeBookingPolicyException_ShouldReturnConflict()
    {
        // Arrange
        var newGuid = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var roomType = RoomTypes.Deluxe;
        var dateTime = DateTime.Today;
        var checkOut = DateTime.Today.AddDays(1);
        var bookingData = new BookingBody(newGuid, employeeId, roomType, dateTime, checkOut);

        _bookUseCaseMock.Setup(x => x.Execute(newGuid, employeeId, roomType, dateTime, checkOut))
            .Throws<EmployeeBookingPolicyException>();

        // Act
        var addBookingResponse = _bookingsController.Book(bookingData);

        // Assert
        ((ConflictResult)addBookingResponse).StatusCode.Should().Be((int)HttpStatusCode.Conflict);
    }


    [Fact]
    public void BookRoom_WhenRoomNotProvidedException_ShouldReturnConflict()
    {
        // Arrange
        var newGuid = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var roomType = RoomTypes.Deluxe;
        var dateTime = DateTime.Today;
        var checkOut = DateTime.Today.AddDays(1);
        var bookingData = new BookingBody(newGuid, employeeId, roomType, dateTime, checkOut);

        _bookUseCaseMock.Setup(x => x.Execute(newGuid, employeeId, roomType, dateTime, checkOut))
            .Throws<RoomTypeNotProvidedException>();

        // Act
        var addBookingResponse = _bookingsController.Book(bookingData);

        // Assert
        ((ConflictResult)addBookingResponse).StatusCode.Should().Be((int)HttpStatusCode.Conflict);
    }


    [Fact]
    public void BookRoom_WhenRoomNotAvailableException_ShouldReturnConflict()
    {
        // Arrange
        var newGuid = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var roomType = RoomTypes.Deluxe;
        var dateTime = DateTime.Today;
        var checkOut = DateTime.Today.AddDays(1);
        var bookingData = new BookingBody(newGuid, employeeId, roomType, dateTime, checkOut);

        _bookUseCaseMock.Setup(x => x.Execute(newGuid, employeeId, roomType, dateTime, checkOut))
            .Throws<RoomTypeNotAvailableException>();

        // Act
        var addBookingResponse = _bookingsController.Book(bookingData);

        // Assert
        ((ConflictResult)addBookingResponse).StatusCode.Should().Be((int)HttpStatusCode.Conflict);
    }


    [Fact]
    public void BookRoom_WhenEmployeeNotFoundException_ShouldReturnConflict()
    {
        // Arrange
        var newGuid = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var roomType = RoomTypes.Deluxe;
        var dateTime = DateTime.Today;
        var checkOut = DateTime.Today.AddDays(1);
        var bookingData = new BookingBody(newGuid, employeeId, roomType, dateTime, checkOut);

        _bookUseCaseMock.Setup(x => x.Execute(newGuid, employeeId, roomType, dateTime, checkOut))
            .Throws<EmployeeNotFoundException>();

        // Act
        var addBookingResponse = _bookingsController.Book(bookingData);

        // Assert
        ((NotFoundResult)addBookingResponse).StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

}