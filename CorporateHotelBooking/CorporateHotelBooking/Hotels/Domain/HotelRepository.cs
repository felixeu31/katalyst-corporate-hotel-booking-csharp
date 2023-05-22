namespace CorporateHotelBooking.Hotels.Domain;

public interface HotelRepository
{
    void AddHotel(int hotelId, string hotelName);
    Hotel GetById(int hotelId);
    void UpdateRoom(int hotelId, int roomNumber, string roomType);
    void AddRoom(int hotelId, int roomNumber, string roomType);
}