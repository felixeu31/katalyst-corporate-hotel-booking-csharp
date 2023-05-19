namespace CorporateHotelBooking;

public class HotelService
{
    private readonly HotelRepository _hotelRepositoryObject;

    public HotelService(HotelRepository hotelRepositoryObject)
    {
        _hotelRepositoryObject = hotelRepositoryObject;
    }

    public void AddHotel(int hotelId, string hotelName)
    {
        _hotelRepositoryObject.AddHotel(hotelId, hotelName);
    }
}