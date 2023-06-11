using Microsoft.AspNetCore.Mvc;
using System.Net;
using CorporateHotelBooking.Application.Hotels.Domain.Exceptions;
using CorporateHotelBooking.Application.Hotels.UseCases;

namespace CorporateHotelBooking.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IAddHotelUseCase _addHotelUseCase;
        private readonly ISetRoomUseCase _setRoomUseCase;
        private readonly IFindHotelUseCase _findHotelUseCase;

        public HotelsController(IAddHotelUseCase addHotelUseCase, ISetRoomUseCase setRoomUseCase, IFindHotelUseCase findHotelUseCase)
        {
            _addHotelUseCase = addHotelUseCase;
            _setRoomUseCase = setRoomUseCase;
            _findHotelUseCase = findHotelUseCase;
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


        [HttpGet("{hotelId}")]
        public IActionResult FindHotelBy(Guid hotelId)
        {
            try
            {
                var hotelDto = _findHotelUseCase.Execute(hotelId);

                return Ok(hotelDto);
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
