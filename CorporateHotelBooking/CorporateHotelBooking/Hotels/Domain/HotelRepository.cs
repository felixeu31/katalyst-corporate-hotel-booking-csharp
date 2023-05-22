namespace CorporateHotelBooking.Hotels.Domain;

public interface IHotelRepository
{
    void Add(Hotel hotel);
    Hotel GetById(int hotelId);
    void Update(Hotel hotel);
}