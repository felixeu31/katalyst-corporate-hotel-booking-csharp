using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Hotels.UseCases;
using CorporateHotelBooking.Data.Sql.DataModel;

namespace CorporateHotelBooking.Data.Sql.Mappers;

public class BookingDataMapper
{
    public static BookingData? MapBookingDataFrom(Booking? booking)
    {
        if (booking is null) return null;

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

    public static Booking? HydrateDomainFrom(BookingData? bookingData)
    {
        if (bookingData is null) return null;

        return new Booking(BookingId.From(bookingData.BookingId),  bookingData.RoomNumber, HotelId.From(bookingData.HotelId), EmployeeId.From(bookingData.BookedBy), bookingData.RoomType, bookingData.CheckIn, bookingData.CheckOut);
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