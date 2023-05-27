using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Hotels.Infrastructure;

public class InMemoryHotelRepository : IHotelRepository
{
    private readonly Dictionary<HotelId, Hotel> _hotels;

    public InMemoryHotelRepository()
    {
        _hotels = new Dictionary<HotelId, Hotel>();
    }

    public void Add(Hotel hotel)
    {
        _hotels.Add(hotel.HotelId, hotel);
    }

    public Hotel? Get(HotelId hotelId)
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