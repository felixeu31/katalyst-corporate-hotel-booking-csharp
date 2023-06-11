using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Hotels.UseCases;

namespace CorporateHotelBooking.Application.Hotels.UseCases;

public record RoomDto(int RoomNumber, string RoomType)
{
    public static RoomDto From(Room room)
    {
        return new RoomDto(room.RoomNumber, room.RoomType);
    }
}
