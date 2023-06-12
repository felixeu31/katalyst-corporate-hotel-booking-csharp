using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Hotels.Domain.Exceptions;
using CorporateHotelBooking.Application.Hotels.UseCases;

namespace CorporateHotelBooking.Application.Hotels.UseCases;

public class FindHotelUseCase : IFindHotelUseCase
{
    private readonly IHotelRepository _hotelRepository;

    public FindHotelUseCase(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public HotelDto Execute(Guid hotelId)
    {
        var hotel = _hotelRepository.Get(HotelId.From(hotelId)) ?? throw new HotelNotFoundException();

        return HotelDto.From(hotel);
    }
}
