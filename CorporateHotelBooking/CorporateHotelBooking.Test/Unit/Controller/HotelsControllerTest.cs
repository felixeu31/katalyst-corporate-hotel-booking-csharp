using CorporateHotelBooking.Api.Controllers;
using CorporateHotelBooking.Hotels.Application;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Test.Unit.Controller;

public class HotelsControllerTest
{
    private Mock<IAddHotelUseCase> _addHotelUseCaseMock;
    private Mock<ISetRoomUseCase> _setRoomUseCaseMock;
    private HotelsController _hotelsController;

    public HotelsControllerTest()
    {
        _addHotelUseCaseMock = new Mock<IAddHotelUseCase>();
        _setRoomUseCaseMock = new Mock<ISetRoomUseCase>();
        _hotelsController = new HotelsController(_addHotelUseCaseMock.Object, _setRoomUseCaseMock.Object);
    }
    [Fact]
    public void ShouldAddTask()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var hotelName = "Westing";

        // Act
        var addHotelResponse = _hotelsController.AddHotel(new AddHotelBody(hotelId, hotelName));

        // Assert
        _addHotelUseCaseMock.Verify(mock => mock.Execute(hotelId, hotelName), Times.Once);
        ((StatusCodeResult)addHotelResponse).StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    [Fact]
    public void ShouldSetRoom()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var setRoomBody = new SetRoomBody(1, "Deluxe");

        // Act
        var addHotelResponse = _hotelsController.SetRoom(hotelId, setRoomBody);

        // Assert
        _setRoomUseCaseMock.Verify(mock => mock.Execute(hotelId, setRoomBody.RoomNumber, setRoomBody.RoomType), Times.Once);
        ((StatusCodeResult)addHotelResponse).StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public void should_return_not_found_when_hotel_does_not_exist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var setRoomBody = new SetRoomBody(1, "Deluxe");

        _setRoomUseCaseMock
            .Setup(x => x.Execute(
                hotelId,
                setRoomBody.RoomNumber,
                setRoomBody.RoomType)).Throws(new HotelNotFoundException());
        // Act
        var setRoomResponse = _hotelsController.SetRoom(hotelId, setRoomBody);

        // Assert

        ((StatusCodeResult)setRoomResponse).StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

}