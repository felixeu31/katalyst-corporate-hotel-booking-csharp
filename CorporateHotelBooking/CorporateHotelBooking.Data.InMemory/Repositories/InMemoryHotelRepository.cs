using CorporateHotelBooking.Application.Hotels.Domain;

namespace CorporateHotelBooking.Data.InMemory.Repositories;

public class InMemoryHotelRepository : IHotelRepository
{
    private readonly InMemoryContext _context;

    public InMemoryHotelRepository(InMemoryContext context)
    {
        _context = context;
    }

    public void Add(Hotel hotel)
    {
        _context.Hotels.Add(hotel.HotelId, hotel);
    }

    public Hotel? Get(HotelId hotelId)
    {
        if (_context.Hotels.TryGetValue(hotelId, out var hotel))
        {
            return hotel;
        }
        return null;
    }

    public void Update(Hotel hotel)
    {
        _context.Hotels[hotel.HotelId] = hotel;
    }
}