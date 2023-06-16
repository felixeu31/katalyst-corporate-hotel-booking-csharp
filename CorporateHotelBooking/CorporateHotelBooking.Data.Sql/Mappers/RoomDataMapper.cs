using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Hotels.UseCases;
using CorporateHotelBooking.Data.Sql.DataModel;

namespace CorporateHotelBooking.Data.Sql.Mappers;

public class RoomDataMapper
{
    public static RoomData? MapRoomDataFrom(Room? room, Guid hotelId)
    {
        if (room is null) return null;

        return new RoomData
        {
            HotelId = hotelId,
            RoomType = room.RoomType,
            RoomNumber = room.RoomNumber
        };
    }

    public static Room? HydrateDomainFrom(RoomData? roomData)
    {
        if (roomData is null) return null;

        return new Room(roomData.RoomNumber, roomData.RoomType);
    }

    public static void ApplyDomainChanges(RoomData roomData, Room room)
    {
        roomData.RoomType = room.RoomType;
    }
}