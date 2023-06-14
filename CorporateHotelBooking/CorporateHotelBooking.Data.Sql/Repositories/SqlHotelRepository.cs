using CorporateHotelBooking.Application.Hotels.Domain;

namespace CorporateHotelBooking.Data.Sql.Repositories;

public class SqlHotelRepository : IHotelRepository
{
    private readonly CorporateHotelDbContext _context;

    public SqlHotelRepository(CorporateHotelDbContext context)
    {
        _context = context;
    }


    public void Add(Hotel hotel)
    {
        throw new NotImplementedException();
    }

    public Hotel? Get(HotelId hotelId)
    {
        throw new NotImplementedException();
    }

    public void Update(Hotel hotel)
    {
        throw new NotImplementedException();
    }
}