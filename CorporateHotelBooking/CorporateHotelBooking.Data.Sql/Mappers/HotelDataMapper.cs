using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Hotels.UseCases;
using CorporateHotelBooking.Data.Sql.DataModel;

namespace CorporateHotelBooking.Data.Sql.Mappers;

public class HotelDataMapper
{
    public static HotelData? MapHotelDataFrom(Hotel? employee)
    {
        if (employee is null) return null;

        return new HotelData
        {
            HotelId = employee.HotelId.Value,
            HotelName = employee.HotelName
        };
    }

    public static Hotel? HydrateDomainFrom(HotelData? hotelData)
    {
        if (hotelData is null) return null;

        var hotel = new Hotel(HotelId.From(hotelData.HotelId), hotelData.HotelName, hotelData.Rooms.Select(RoomDataMapper.HydrateDomainFrom).ToList());

        return hotel;
    }

    public static void ApplyDomainChanges(HotelData hotelData, Hotel hotel)
    {
        hotelData.HotelName = hotel.HotelName;

        ApplyCollectionChanges(hotelData.Rooms, hotel.Rooms, hotelData.HotelId);
    }

    private static void ApplyCollectionChanges(ICollection<RoomData> roomDatas, IReadOnlyList<Room> rooms, Guid hotelId)
    {
        // Updated
        foreach (var updatedRoom in rooms.Where(x => roomDatas.Any(y => y.RoomNumber == x.RoomNumber)))
        {
            var roomData = roomDatas.FirstOrDefault(x => x.RoomNumber == updatedRoom.RoomNumber);

            RoomDataMapper.ApplyDomainChanges(roomData, updatedRoom);
        }

        // Added
        foreach (var addedRoom in rooms.Where(x => roomDatas.All(y => y.RoomNumber != x.RoomNumber)))
        {
            roomDatas.Add(RoomDataMapper.MapRoomDataFrom(addedRoom, hotelId));
        }

        // Deleted
        foreach (var deletedRoom in roomDatas.Where(x => rooms.All(y => y.RoomNumber != x.RoomNumber)))
        {
            roomDatas.Remove(deletedRoom);
        }
    }
}