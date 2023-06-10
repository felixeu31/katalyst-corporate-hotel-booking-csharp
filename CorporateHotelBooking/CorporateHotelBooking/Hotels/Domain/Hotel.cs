namespace CorporateHotelBooking.Hotels.Domain;

public class Hotel
{
    public Hotel(HotelId hotelId, string hotelName)
    {
        HotelId = hotelId;
        HotelName = hotelName;
        _rooms = new List<Room>();
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

    public int GetAvailableRoom(string roomType)
    {
        return _rooms.First(x => x.RoomType.Equals(roomType)).RoomNumber;
    }
}