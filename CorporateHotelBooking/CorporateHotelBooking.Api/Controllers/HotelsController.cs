using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CorporateHotelBooking.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        [HttpPost]
        public IActionResult AddHotel(AddHotelBody hotelData)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{hotelId}/rooms")]
        public IActionResult SetRoom(Guid hotelId, SetRoomBody roomData)
        {
            throw new NotImplementedException();
        }
    }
    
    public record AddHotelBody(Guid HotelId, string HotelName) { }
    public record SetRoomBody(int RoomNumber, string RoomType) { }
}
