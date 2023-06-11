using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Hotels.UseCases;

namespace CorporateHotelBooking.Application.Hotels.UseCases;

public record HotelDto(Guid HotelId, string HotelName, Dictionary<string, int> RoomCount, List<RoomDto> Rooms)
{
    public static HotelDto From(Hotel hotel)
    {
        return new HotelDto(hotel.HotelId.Value, hotel.HotelName, hotel.CalculateRoomCount(),
            hotel.Rooms.Select(RoomDto.From).ToList());
    }
}
