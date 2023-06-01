using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Hotels.Application;

public record HotelDto(Guid HotelId, string HotelName, Dictionary<string, int> RoomCount, List<RoomDto> Rooms)
{
    public static HotelDto From(Hotel hotel)
    {
        return new HotelDto(hotel.HotelId.Value, hotel.HotelName, hotel.CalculateRoomCount(),
            hotel.Rooms.Select(RoomDto.From).ToList());
    }
}