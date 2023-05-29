using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CorporateHotelBooking.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        [HttpPost]
        public IActionResult Book(BookBody bookingData)
        {
            throw new NotImplementedException();
        }
    }

    public record BookBody(int RoomNumber, Guid HotelId, Guid EmployeeId, string RoomType, DateTime CheckIn, DateTime CheckOut) { }
}
