namespace CorporateHotelBooking.Hotels.Application;

public interface ISetRoomUseCase
{
    void Execute(Guid hotelId, int roomNumber, string roomType);
}