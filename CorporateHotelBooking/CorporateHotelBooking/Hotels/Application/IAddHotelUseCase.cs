namespace CorporateHotelBooking.Hotels.Application;

public interface IAddHotelUseCase
{
    void Execute(Guid hotelId, string hotelName);
}