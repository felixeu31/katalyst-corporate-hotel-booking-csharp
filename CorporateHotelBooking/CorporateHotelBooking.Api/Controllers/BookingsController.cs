using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using CorporateHotelBooking.Bookings.Application;
using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Test.Unit.Controller;

namespace CorporateHotelBooking.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookUseCase _bookUseCase;

        public BookingsController(IBookUseCase bookUseCase)
        {
            _bookUseCase = bookUseCase;
        }

        [HttpPost]
        public IActionResult Book(BookingBody bookingData)
        {
            try
            {
                var booking = _bookUseCase.Execute(bookingData.HotelId,
                    bookingData.EmployeeId,
                    bookingData.RoomType,
                    bookingData.CheckIn,
                    bookingData.CheckOut);

                return Created("", BookingDto.From(booking));
            }
            catch (EmployeeBookingPolicyException)
            {
                return Conflict();
            }
        }
    }

    public record BookingBody(Guid HotelId, Guid EmployeeId, string RoomType, DateTime CheckIn, DateTime CheckOut) { }

    public record BookingDto(Guid BookingId, Guid HotelId, Guid BookedBy, int RoomNumber, string RoomType,
        DateTime CheckIn, DateTime CheckOut)
    {
        public static BookingDto? From(Booking? booking)
        {
            if (booking == null) return null;

            return new BookingDto(
                booking.BookingId.Value,
                booking.HotelId.Value,
                booking.BookedBy.Value,
                booking.RoomNumber,
                booking.RoomType,
                booking.CheckIn,
                booking.CheckOut);
        }
    }
}
