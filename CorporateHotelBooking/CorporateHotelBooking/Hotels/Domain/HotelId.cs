using ValueOf;

namespace CorporateHotelBooking.Hotels.Domain;

public class HotelId : ValueOf<Guid, HotelId>
{
    public static HotelId New()
    {
        return From(Guid.NewGuid());
    }
}