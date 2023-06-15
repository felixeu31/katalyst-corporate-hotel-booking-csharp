using CorporateHotelBooking.Data.Sql.DataModel;
using Microsoft.EntityFrameworkCore;

namespace CorporateHotelBooking.Data.Sql;

public class CorporateHotelDbContext : DbContext
{
    public DbSet<EmployeeData> Employees { get; set; }
    public DbSet<HotelData> Hotels { get; set; }
    public DbSet<RoomData> Rooms { get; set; }
    public DbSet<BookingData> Bookings { get; set; }

    public CorporateHotelDbContext(DbContextOptions<CorporateHotelDbContext> options) : base(options)
    {
    }
}