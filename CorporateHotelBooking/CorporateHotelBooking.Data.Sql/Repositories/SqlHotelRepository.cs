using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Data.Sql.Mappers;
using Microsoft.EntityFrameworkCore;

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
        var hotelData = HotelDataMapper.MapHotelDataFrom(hotel);

        _context.Hotels.Add(hotelData);

        _context.SaveChanges();
    }

    public Hotel? Get(HotelId hotelId)
    {
        var hotelData = _context.Hotels.Include(x => x.Rooms).FirstOrDefault(x => x.HotelId == hotelId.Value);

        var hotel = HotelDataMapper.HydrateDomainFrom(hotelData);

        return hotel;
    }

    public void Update(Hotel hotel)
    {
        var hotelData = _context.Hotels.Include(x => x.Rooms).FirstOrDefault(x => x.HotelId == hotel.HotelId.Value);

        HotelDataMapper.ApplyDomainChanges(hotelData, hotel);

        _context.SaveChanges();
    }
}