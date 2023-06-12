using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Hotels.Domain.Exceptions;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Application.Policies.UseCases;

namespace CorporateHotelBooking.Application.Bookings.UseCases;

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

        ThrowIfPolicyDoesNotAllowBooking(employeeId, roomType);

        var existingBookings = _bookingRepository.GetBookingsBy(hotel.HotelId);

        var roomNumber = hotel.GetAvailableRoom(roomType, checkIn, checkOut, existingBookings);

        var booking = new Booking(roomNumber, HotelId.From(hotelId), EmployeeId.From(employeeId), roomType, checkIn,
            checkOut);

        _bookingRepository.Add(booking);

        return booking;
    }

    private void ThrowIfPolicyDoesNotAllowBooking(Guid employeeId, string roomType)
    {
        if (!_isBookingAllowedUseCase.Execute(employeeId, roomType))
        {
            throw new EmployeeBookingPolicyException();
        }
    }
}
