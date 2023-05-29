using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Bookings.Application;

public class BookUseCase
{
    private readonly IBookingRepository _bookingRepository;

    public BookUseCase(IBookingRepository bookingRepository)
    {

        _bookingRepository = bookingRepository;
    }

    public Booking Execute(int roomNumber, Guid hotelId, Guid employeeId, string roomType, DateTime checkIn,
        DateTime checkOut)
    {
        var booking = new Booking(roomNumber, HotelId.From(hotelId), EmployeeId.From(employeeId), roomType, checkIn,
            checkOut);
        _bookingRepository.Add(booking);
        return booking;
    }
}