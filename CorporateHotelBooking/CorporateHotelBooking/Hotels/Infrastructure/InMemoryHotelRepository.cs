using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Hotels.Infrastructure;

public class InMemoryHotelRepository : IHotelRepository
{
    private readonly List<Hotel> _hotels;

    public InMemoryHotelRepository()
    {
        _hotels = new List<Hotel>();
    }

    public void Add(Hotel hotel)
    {
        _hotels.Add(hotel);
    }

    public Hotel GetById(int hotelId)
    {
        return _hotels.First(x => x.HotelId.Equals(hotelId));
    }

    public void Update(Hotel hotel)
    {
        
    }
}