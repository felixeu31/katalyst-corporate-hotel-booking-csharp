using CorporateHotelBooking.Application.Bookings.Domain.Exceptions;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;

namespace CorporateHotelBooking.Application.Bookings.Domain;

public class Booking
{
    public Booking(int roomNumber, HotelId hotelId, EmployeeId bookedBy, string roomType, DateTime checkIn, DateTime checkOut)
    {
        if (checkOut < checkIn || (checkOut - checkIn).TotalDays < 1)
        {
            throw new InvalidBookingPeriodException();
        }

        BookingId = BookingId.New();
        RoomNumber = roomNumber;
        HotelId = hotelId;
        BookedBy = bookedBy;
        RoomType = roomType;
        CheckIn = checkIn;
        CheckOut = checkOut;
    }

    public BookingId BookingId { get; set; }
    public HotelId HotelId { get; set; }
    public EmployeeId BookedBy { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public string RoomType { get; set; }
    public int RoomNumber { get; set; }
}
