using CorporateHotelBooking.Test.Constants;
using FluentAssertions;
using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Bookings.Domain.Exceptions;
using CorporateHotelBooking.Application.Hotels.Domain;

namespace CorporateHotelBooking.Test.Unit.Domain;

public class HotelTest
{
    [Fact]
    public void ShouldAddRoom_WhenNotExists()
    {
        // Arrange
        var hotel = new Hotel(new HotelId(Guid.NewGuid()), "Westing");

        // Act
        hotel.SetRoom(1, SampleData.RoomTypes.Deluxe);

        // Assert
        hotel.Rooms.Should().HaveCount(1);
        hotel.Rooms[0].RoomNumber.Should().Be(1);
        hotel.Rooms[0].RoomType.Should().Be(SampleData.RoomTypes.Deluxe);
    }

    [Fact]
    public void ShouldUpdateRoom_WhenAlreadyExists()
    {
        // Arrange
        var hotel = new Hotel(new HotelId(Guid.NewGuid()), "Westing");

        // Act
        hotel.SetRoom(1, SampleData.RoomTypes.Deluxe);
        hotel.SetRoom(1, SampleData.RoomTypes.Standard);

        // Assert
        hotel.Rooms.Should().HaveCount(1);
        hotel.Rooms[0].RoomNumber.Should().Be(1);
        hotel.Rooms[0].RoomType.Should().Be(SampleData.RoomTypes.Standard);
    }

    [Fact]
    public void ShouldCalculateRoomCount()
    {
        // Arrange
        var hotel = new Hotel(new HotelId(Guid.NewGuid()), "Westing");
        hotel.SetRoom(1, SampleData.RoomTypes.Deluxe);
        hotel.SetRoom(2, SampleData.RoomTypes.Deluxe);
        hotel.SetRoom(3, SampleData.RoomTypes.Standard);

        // Act
        var roomCount = hotel.CalculateRoomCount();

        // Assert
        roomCount.Should().HaveCount(2);
        roomCount[SampleData.RoomTypes.Deluxe].Should().Be(2);
        roomCount[SampleData.RoomTypes.Standard].Should().Be(1);
    }

    [Fact]
    public void ShouldGetAvailableRoom()
    {
        var hotel = new Hotel(new HotelId(Guid.NewGuid()), "Westing");
        hotel.SetRoom(1, SampleData.RoomTypes.Deluxe);
        hotel.SetRoom(2, SampleData.RoomTypes.Deluxe);
        hotel.SetRoom(3, SampleData.RoomTypes.Standard);

        // Act
        var nextRoomNumber = hotel.GetAvailableRoom(SampleData.RoomTypes.Deluxe, DateTime.Today, DateTime.Today.AddDays(1), new List<Booking>());

        // Assert
        nextRoomNumber.Should().Be(1);
    }

    [Fact]
    public void GetAvailableRoom_WhenRoomTypeNotProvided_ShouldThrowRoomNotProvidedException()
    {
        // Arrange
        var hotel = new Hotel(new HotelId(Guid.NewGuid()), "Westing");
        hotel.SetRoom(1, SampleData.RoomTypes.Deluxe);
        hotel.SetRoom(2, SampleData.RoomTypes.Deluxe);
        hotel.SetRoom(3, SampleData.RoomTypes.Standard);

        // Act
        Action action = () => hotel.GetAvailableRoom(SampleData.RoomTypes.Presidential, DateTime.Today, DateTime.Today.AddDays(1), new List<Booking>());

        // Assert
        action.Should().Throw<RoomTypeNotProvidedException>();
    }
}