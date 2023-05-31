using CorporateHotelBooking.Hotels.Application;
using CorporateHotelBooking.Hotels.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CorporateHotelBooking.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IAddHotelUseCase _addHotelUseCase;
        private readonly ISetRoomUseCase _setRoomUseCase;

        public HotelsController(IAddHotelUseCase addHotelUseCase, ISetRoomUseCase setRoomUseCase)
        {
            _addHotelUseCase = addHotelUseCase;
            _setRoomUseCase = setRoomUseCase;
        }

        [HttpPost]
        public IActionResult AddHotel(AddHotelBody hotelData)
        {
            try
            {
                _addHotelUseCase.Execute(hotelData.HotelId, hotelData.HotelName);

                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (ExistingHotelException)
            {
                return StatusCode((int)HttpStatusCode.Conflict);
            }
        }

        [HttpPost("{hotelId}/rooms")]
        public IActionResult SetRoom(Guid hotelId, SetRoomBody roomData)
        {
            try
            {
                _setRoomUseCase.Execute(hotelId, roomData.RoomNumber, roomData.RoomType);

                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (HotelNotFoundException)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
        }
    }
    
    public record AddHotelBody(Guid HotelId, string HotelName) { }
    public record SetRoomBody(int RoomNumber, string RoomType) { }
}
