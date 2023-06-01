using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Hotels.Application;

public record RoomDto(int RoomNumber, string RoomType)
{
    public static RoomDto From(Room room)
    {
        return new RoomDto(room.RoomNumber, room.RoomType);
    }
}