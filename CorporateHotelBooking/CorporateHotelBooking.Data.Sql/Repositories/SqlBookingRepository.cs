using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Data.Sql.Mappers;

namespace CorporateHotelBooking.Data.Sql.Repositories;

public class SqlBookingRepository : IBookingRepository
{
    private readonly CorporateHotelDbContext _context;

    public SqlBookingRepository(CorporateHotelDbContext context)
    {
        _context = context;
    }


    public void Add(Booking booking)
    {
        var bookingData = BookingDataMapper.MapBookingDataFrom(booking);

        _context.Bookings.Add(bookingData);

        _context.SaveChanges();
    }

    public Booking? Get(BookingId bookingId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Booking> GetBookingsBy(HotelId hotelId)
    {
        throw new NotImplementedException();
    }

    public void DeleteEmployeeBookings(EmployeeId employeeId)
    {
        throw new NotImplementedException();
    }
}