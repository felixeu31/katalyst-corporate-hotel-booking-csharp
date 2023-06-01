using CorporateHotelBooking.Hotels.Domain;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Unit.Domain;

public class HotelTest
{
    [Fact]
    public void ShouldAddRoom_WhenNotExists()
    {
        // Arrange
        var hotel = new Hotel(new HotelId(Guid.NewGuid()), "Westing");

        // Act
        hotel.SetRoom(1, "Deluxe");

        // Assert
        hotel.Rooms.Should().HaveCount(1);
        hotel.Rooms[0].RoomNumber.Should().Be(1);
        hotel.Rooms[0].RoomType.Should().Be("Deluxe");
    }

    [Fact]
    public void ShouldUpdateRoom_WhenAlreadyExists()
    {
        // Arrange
        var hotel = new Hotel(new HotelId(Guid.NewGuid()), "Westing");

        // Act
        hotel.SetRoom(1, "Deluxe");
        hotel.SetRoom(1, "Standard");

        // Assert
        hotel.Rooms.Should().HaveCount(1);
        hotel.Rooms[0].RoomNumber.Should().Be(1);
        hotel.Rooms[0].RoomType.Should().Be("Standard");
    }

    [Fact]
    public void ShouldCalculateRoomCount()
    {
        // Arrange
        var hotel = new Hotel(new HotelId(Guid.NewGuid()), "Westing");
        hotel.SetRoom(1, "Deluxe");
        hotel.SetRoom(2, "Deluxe");
        hotel.SetRoom(3, "Standard");

        // Act
        var roomCount = hotel.CalculateRoomCount();

        // Assert
        roomCount.Should().HaveCount(2);
        roomCount["Deluxe"].Should().Be(2);
        roomCount["Standard"].Should().Be(1);
    }
}