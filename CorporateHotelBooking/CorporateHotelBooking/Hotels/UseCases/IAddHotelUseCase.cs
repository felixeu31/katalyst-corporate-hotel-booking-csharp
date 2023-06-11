namespace CorporateHotelBooking.Application.Hotels.UseCases;

public interface IAddHotelUseCase
{
    void Execute(Guid hotelId, string hotelName);
}
