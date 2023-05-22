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
    public List<Room> Rooms { get; set; }

    public void AddRoom(Room room)
    {
        Rooms.Add(room);
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