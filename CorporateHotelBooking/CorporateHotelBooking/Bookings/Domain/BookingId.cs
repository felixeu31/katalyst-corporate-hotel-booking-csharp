using ValueOf;

namespace CorporateHotelBooking.Bookings.Domain;

public class BookingId : ValueOf<Guid, BookingId>
{
    public static BookingId New()
    {
        return From(Guid.NewGuid());
    }
}