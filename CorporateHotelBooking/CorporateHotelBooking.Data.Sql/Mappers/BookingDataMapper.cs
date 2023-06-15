using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Data.Sql.DataModel;

namespace CorporateHotelBooking.Data.Sql.Mappers;

public class BookingDataMapper
{
    public static BookingData MapBookingDataFrom(Booking booking)
    {
        return new BookingData
        {
            BookingId = booking.BookingId.Value,
            HotelId = booking.HotelId.Value,
            BookedBy = booking.BookedBy.Value,
            CheckIn = booking.CheckIn,
            CheckOut = booking.CheckOut,
            RoomType = booking.RoomType,
            RoomNumber = booking.RoomNumber
        };
    }

    public static Booking HydrateDomainFrom(BookingData bookingData)
    {
        return new Booking(bookingData.RoomNumber, HotelId.From(bookingData.HotelId), EmployeeId.From(bookingData.BookedBy), bookingData.RoomType, bookingData.CheckIn, bookingData.CheckOut);
    }

    public static void ApplyDomainChanges(BookingData bookingData, Booking booking)
    {
        bookingData.HotelId = booking.HotelId.Value;
        bookingData.BookedBy = booking.BookedBy.Value;
        bookingData.CheckIn = booking.CheckIn;
        bookingData.CheckOut = booking.CheckOut;
        bookingData.RoomNumber = booking.RoomNumber;
        bookingData.RoomType = booking.RoomType;
    }
}