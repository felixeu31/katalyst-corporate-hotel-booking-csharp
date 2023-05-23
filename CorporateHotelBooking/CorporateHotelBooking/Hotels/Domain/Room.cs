namespace CorporateHotelBooking.Hotels.Domain;

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