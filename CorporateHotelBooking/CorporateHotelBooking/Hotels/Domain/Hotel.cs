namespace CorporateHotelBooking.Hotels.Domain;

public class Hotel
{
    public Hotel(int hotelId, string hotelName)
    {
        HotelId = hotelId;
        HotelName = hotelName;
    }

    public int HotelId { get; set; }
    public string HotelName { get; set; }
}