using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Hotels.Infrastructure;

public class InMemoryHotelRepository : HotelRepository
{
    private readonly List<Hotel> _hotels;

    public InMemoryHotelRepository()
    {
        _hotels = new List<Hotel>();
    }

    public void AddHotel(int hotelId, string hotelName)
    {
        _hotels.Add(new Hotel(hotelId, hotelName));
    }

    public Hotel GetById(int hotelId)
    {
        return _hotels.First(x => x.HotelId.Equals(hotelId));
    }

    public void UpdateRoom(int hotelId, int roomNumber, string roomType)
    {
        var hotel = GetById(hotelId);

        var room = hotel.Rooms.First(x => x.RoomNumber.Equals(roomNumber));

        room.RoomType = roomType;
    }

    public void AddRoom(int hotelId, int roomNumber, string roomType)
    {
        var hotel = GetById(hotelId);

        hotel.Rooms.Add(new Room(roomNumber, roomType));
    }
}