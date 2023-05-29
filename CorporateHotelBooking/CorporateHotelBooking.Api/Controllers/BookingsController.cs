using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using CorporateHotelBooking.Bookings.Application;

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
        public IActionResult Book(BookBody bookingData)
        {
            var booking = _bookUseCase.Execute(bookingData.RoomNumber,
                bookingData.HotelId,
                bookingData.EmployeeId,
                bookingData.RoomType,
                bookingData.CheckIn,
                bookingData.CheckOut);

            return Created("", booking);
        }
    }

    public record BookBody(int RoomNumber, Guid HotelId, Guid EmployeeId, string RoomType, DateTime CheckIn, DateTime CheckOut) { }
}
