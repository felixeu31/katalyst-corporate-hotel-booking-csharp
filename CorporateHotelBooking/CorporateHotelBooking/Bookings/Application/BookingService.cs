using CorporateHotelBooking.Bookings.Domain;

namespace CorporateHotelBooking.Bookings.Application;

public class BookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {

        _bookingRepository = bookingRepository;
    }

    public Booking Book(int roomNumber, int hotelId, int employeeId, string roomType, DateTime checkIn,
        DateTime checkOut)
    {
        var booking = new Booking(roomNumber, hotelId, employeeId, roomType, checkIn,
            checkOut);
        _bookingRepository.Add(booking);
        return booking;
    }
}