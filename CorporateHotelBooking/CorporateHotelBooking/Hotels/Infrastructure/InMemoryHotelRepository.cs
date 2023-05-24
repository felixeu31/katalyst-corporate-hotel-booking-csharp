using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Hotels.Infrastructure;

public class InMemoryHotelRepository : IHotelRepository
{
    private readonly Dictionary<int, Hotel> _hotels;

    public InMemoryHotelRepository()
    {
        _hotels = new Dictionary<int, Hotel>();
    }

    public void Add(Hotel hotel)
    {
        _hotels.Add(hotel.HotelId, hotel);
    }

    public Hotel? Get(int hotelId)
    {
        if (_hotels.TryGetValue(hotelId, out var hotel))
        {
            return hotel;
        }
        return null;
    }

    public void Update(Hotel hotel)
    {
        _hotels[hotel.HotelId] = hotel;
    }
}