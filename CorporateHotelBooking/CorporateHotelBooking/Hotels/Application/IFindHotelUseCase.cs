namespace CorporateHotelBooking.Hotels.Application;

public interface IFindHotelUseCase
{
    HotelDto Execute(Guid hotelId);
}