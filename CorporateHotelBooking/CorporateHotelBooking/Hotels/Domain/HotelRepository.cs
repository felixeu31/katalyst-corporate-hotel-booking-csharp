namespace CorporateHotelBooking.Hotels.Domain;

public interface IHotelRepository
{
    void Add(Hotel hotel);
    Hotel? Get(HotelId hotelId);
    void Update(Hotel hotel);
}