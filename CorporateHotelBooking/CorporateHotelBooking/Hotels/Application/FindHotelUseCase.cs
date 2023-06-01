using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Hotels.Application;

public class FindHotelUseCase : IFindHotelUseCase
{
    private readonly IHotelRepository _hotelRepository;

    public FindHotelUseCase(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public HotelDto Execute(Guid hotelId)
    {
        var hotel = _hotelRepository.Get(HotelId.From(hotelId));

        if (hotel == null)
        {
            throw new HotelNotFoundException();
        }

        return HotelDto.From(hotel);
    }
}