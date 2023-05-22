namespace CorporateHotelBooking.Hotels.Domain;

public class Hotel
{
    public Hotel(int hotelId, string hotelName)
    {
        HotelId = hotelId;
        HotelName = hotelName;
        Rooms = new List<Room>();
    }

    public int HotelId { get; set; }
    public string HotelName { get; set; }
    public List<Room> Rooms { get; private set; }

    public void SetRoom(int roomNumber, string roomType)
    {
        if (Rooms.Exists(x => x.RoomNumber == roomNumber))
        {
            UpdateRoom(roomNumber, roomType);
        }

        AddRoom(roomNumber, roomType);
    }

    public void UpdateRoom(int roomNumber, string roomType)
    {
        var room = Rooms.FirstOrDefault(x => x.RoomNumber.Equals(roomNumber));

        room.RoomType = roomType;
    }

    public void AddRoom(int roomNumber, string roomType)
    {
        Rooms.Add(new Room(roomNumber, roomType));
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
}