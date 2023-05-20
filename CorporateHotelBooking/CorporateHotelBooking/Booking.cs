namespace CorporateHotelBooking;

public class Booking
{
    public int HotelId { get; set; }
    public int BookedBy { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public string RoomType { get; set; }
    public int RoomNumber { get; set; }
}