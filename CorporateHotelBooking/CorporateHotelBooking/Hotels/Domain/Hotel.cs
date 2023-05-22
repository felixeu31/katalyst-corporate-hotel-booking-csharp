namespace CorporateHotelBooking.Hotels.Domain;

public class Hotel
{
    public Hotel(int hotelId, string hotelName)
    {
        HotelId = hotelId;
        HotelName = hotelName;
        _rooms = new List<Room>();
    }

    public int HotelId { get; }

    public string HotelName { get; }

    private readonly List<Room> _rooms;

    public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();

    public void SetRoom(int roomNumber, string roomType)
    {
        if (_rooms.Exists(x => x.RoomNumber == roomNumber))
        {
            UpdateRoom(roomNumber, roomType);
        }

        AddRoom(roomNumber, roomType);
    }

    public void UpdateRoom(int roomNumber, string roomType)
    {
        var room = _rooms.FirstOrDefault(x => x.RoomNumber.Equals(roomNumber));

        room.SetRoomType(roomType);
    }

    public void AddRoom(int roomNumber, string roomType)
    {
        _rooms.Add(new Room(roomNumber, roomType));
    }
}

public class Room
{
    public Room(int roomNumber, string roomType)
    {
        RoomNumber = roomNumber;
        RoomType = roomType;
    }

    public int RoomNumber { get; set; }
    public string RoomType { get; set; }

    public void SetRoomType(string roomType)
    {
        RoomType = roomType;
    }
}