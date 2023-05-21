using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Hotels.Infrastructure;

public class InMemoryHotelRepository : HotelRepository
{
    private readonly List<Hotel> _hotels;

    public InMemoryHotelRepository()
    {
        _hotels = new List<Hotel>();
    }

    public void AddHotel(int hotelId, string hotelName)
    {
        _hotels.Add(new Hotel(hotelId, hotelName));
    }

    public Hotel GetById(int hotelId)
    {
        return _hotels.First(x => x.HotelId.Equals(hotelId));
    }
}