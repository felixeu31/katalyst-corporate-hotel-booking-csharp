using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Bookings.Domain;

public class Booking
{
    public Booking(int roomNumber, HotelId hotelId, EmployeeId bookedBy, string roomType, DateTime checkIn, DateTime checkOut)
    {
        BookingId = Guid.NewGuid();
        RoomNumber = roomNumber;
        HotelId = hotelId;
        BookedBy = bookedBy;
        RoomType = roomType;
        CheckIn = checkIn;
        CheckOut = checkOut;
    }

    public Guid BookingId { get; set; }
    public HotelId HotelId { get; set; }
    public EmployeeId BookedBy { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public string RoomType { get; set; }
    public int RoomNumber { get; set; }
}