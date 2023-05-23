namespace CorporateHotelBooking.Hotels.Domain;

public interface IHotelRepository
{
    void Add(Hotel hotel);
    Hotel? Get(int hotelId);
    void Update(Hotel hotel);
}