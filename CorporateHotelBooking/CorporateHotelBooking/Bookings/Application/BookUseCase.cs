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
    private readonly IHotelRepository _hotelRepository;

    public BookUseCase(IBookingRepository bookingRepository, IIsBookingAllowedUseCase isBookingAllowedUseCase, IHotelRepository hotelRepository)
    {
        _bookingRepository = bookingRepository;
        _isBookingAllowedUseCase = isBookingAllowedUseCase;
        _hotelRepository = hotelRepository;
    }

    public Booking Execute(Guid hotelId, Guid employeeId, string roomType, DateTime checkIn,
        DateTime checkOut)
    {
        var hotel = _hotelRepository.Get(HotelId.From(hotelId)) ?? throw new HotelNotFoundException();

        if (!_isBookingAllowedUseCase.Execute(employeeId, roomType))
        {
            throw new EmployeeBookingPolicyException();
        }

        var roomNumber = hotel.GetAvailableRoom(roomType);

        var booking = new Booking(roomNumber, HotelId.From(hotelId), EmployeeId.From(employeeId), roomType, checkIn,
            checkOut);

        _bookingRepository.Add(booking);

        return booking;
    }
}