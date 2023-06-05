using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Hotels.Domain;
using CorporateHotelBooking.Policies.Application;
using CorporateHotelBooking.Test.Unit.Controller;

namespace CorporateHotelBooking.Bookings.Application;

public class BookUseCase : IBookUseCase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IIsBookingAllowedUseCase _isBookingAllowedUseCase;

    public BookUseCase(IBookingRepository bookingRepository, IIsBookingAllowedUseCase isBookingAllowedUseCase)
    {
        _bookingRepository = bookingRepository;
        _isBookingAllowedUseCase = isBookingAllowedUseCase;
    }

    public Booking Execute(int roomNumber, Guid hotelId, Guid employeeId, string roomType, DateTime checkIn,
        DateTime checkOut)
    {
        if (!_isBookingAllowedUseCase.Execute(employeeId, roomType))
        {
            throw new EmployeeBookingPolicyException();
        }

        var booking = new Booking(roomNumber, HotelId.From(hotelId), EmployeeId.From(employeeId), roomType, checkIn,
            checkOut);
        _bookingRepository.Add(booking);
        return booking;
    }
}