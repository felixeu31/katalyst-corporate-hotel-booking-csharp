using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Bookings.Application;

public class BookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {

        _bookingRepository = bookingRepository;
    }

    public Booking Book(int roomNumber, Guid hotelId, int employeeId, string roomType, DateTime checkIn,
        DateTime checkOut)
    {
        var booking = new Booking(roomNumber, HotelId.From(hotelId), employeeId, roomType, checkIn,
            checkOut);
        _bookingRepository.Add(booking);
        return booking;
    }
}