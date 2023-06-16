using System.Linq;
using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Bookings.Domain.Exceptions;
using CorporateHotelBooking.Application.Hotels.Domain;

namespace CorporateHotelBooking.Application.Hotels.Domain;

public class Hotel
{
    public Hotel(HotelId hotelId, string hotelName) : this(hotelId, hotelName, new List<Room>()) { }

    public Hotel(HotelId hotelId, string hotelName, List<Room?> rooms)
    {
        HotelId = hotelId;
        HotelName = hotelName;
        _rooms = rooms;
    }

    public HotelId HotelId { get; }

    public string HotelName { get; }

    private readonly List<Room> _rooms;

    public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();

    public void SetRoom(int roomNumber, string roomType)
    {
        if (_rooms.Exists(x => x.RoomNumber == roomNumber))
        {
            UpdateRoom(roomNumber, roomType);
        }
        else
        {
            AddRoom(roomNumber, roomType);
        }
    }

    private void UpdateRoom(int roomNumber, string roomType)
    {
        var room = _rooms.FirstOrDefault(x => x.RoomNumber.Equals(roomNumber));

        room.SetRoomType(roomType);
    }

    private void AddRoom(int roomNumber, string roomType)
    {
        _rooms.Add(new Room(roomNumber, roomType));
    }

    public Dictionary<string, int> CalculateRoomCount()
    {
        var roomCount = new Dictionary<string, int>();

        foreach (var roomTypeGroup in _rooms.GroupBy(x => x.RoomType))
        {
            roomCount.Add(roomTypeGroup.Key, roomTypeGroup.Count());
        }

        return roomCount;
    }

    public int GetAvailableRoom(string roomType, DateTime checkIn, DateTime checkOut,
        IEnumerable<Booking> existingBookings)
    {
        var roomsOfType = _rooms.Where(x => x.RoomType.Equals(roomType)).ToList();

        if (!roomsOfType.Any())
        {
            throw new RoomTypeNotProvidedException();
        }

        var conflictBookings = existingBookings.Where(x => 
            (checkIn >= x.CheckIn && checkIn <= x.CheckOut)
            || (checkOut >= x.CheckIn && checkOut <= x.CheckOut)
            || (checkIn <= x.CheckIn && checkOut >= x.CheckOut)
        );

        var availableRooms = roomsOfType.Where(x => conflictBookings.All(y => y.RoomNumber != x.RoomNumber)).ToList();

        if (!availableRooms.Any())
        {
            throw new RoomTypeNotAvailableException();
        }

        return availableRooms.First().RoomNumber;
    }
}
