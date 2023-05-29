using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Hotels.Application;

public class AddHotelUseCase
{
    private readonly IHotelRepository _hotelRepository;

    public AddHotelUseCase(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public void Execute(Guid hotelId, string hotelName)
    {
        _hotelRepository.Add(new Hotel(HotelId.From(hotelId), hotelName));
    }
}