using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Hotels.Domain.Exceptions;
using CorporateHotelBooking.Application.Hotels.UseCases;

namespace CorporateHotelBooking.Application.Hotels.UseCases;

public class SetRoomUseCase : ISetRoomUseCase
{
    private readonly IHotelRepository _hotelRepository;

    public SetRoomUseCase(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public void Execute(Guid hotelId, int roomNumber, string roomType)
    {
        var hotel = _hotelRepository.Get(HotelId.From(hotelId));

        if (hotel is null) throw new HotelNotFoundException();

        hotel.SetRoom(roomNumber, roomType);

        _hotelRepository.Update(hotel);
    }
}
