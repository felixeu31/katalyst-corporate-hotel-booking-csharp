using CorporateHotelBooking.Application.Hotels.UseCases;

namespace CorporateHotelBooking.Application.Hotels.UseCases;

public interface IFindHotelUseCase
{
    HotelDto Execute(Guid hotelId);
}
