using CorporateHotelBooking.Hotels.Domain;
using CorporateHotelBooking.Hotels.Domain.Exceptions;

namespace CorporateHotelBooking.Hotels.Application;

public class AddHotelUseCase : IAddHotelUseCase
{
    private readonly IHotelRepository _hotelRepository;

    public AddHotelUseCase(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public void Execute(Guid hotelId, string hotelName)
    {
        if (_hotelRepository.Get(HotelId.From(hotelId)) is not null) throw new ExistingHotelException();

        _hotelRepository.Add(new Hotel(HotelId.From(hotelId), hotelName));
    }
}