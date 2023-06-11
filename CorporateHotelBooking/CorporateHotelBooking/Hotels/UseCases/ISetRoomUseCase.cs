namespace CorporateHotelBooking.Application.Hotels.UseCases;

public interface ISetRoomUseCase
{
    void Execute(Guid hotelId, int roomNumber, string roomType);
}
