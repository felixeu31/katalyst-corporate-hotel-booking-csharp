namespace CorporateHotelBooking.Bookings.Domain;

public class Booking
{
    public Booking(int roomNumber, int hotelId, int bookedBy, string roomType, DateTime checkIn, DateTime checkOut)
    {
        BookingId = Guid.NewGuid();
        RoomNumber = roomNumber;
        HotelId = hotelId;
        BookedBy = bookedBy;
        RoomType = roomType;
        CheckIn = checkIn;
        CheckOut = checkOut;
    }

    public Guid BookingId { get; set; }
    public int HotelId { get; set; }
    public int BookedBy { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public string RoomType { get; set; }
    public int RoomNumber { get; set; }
}