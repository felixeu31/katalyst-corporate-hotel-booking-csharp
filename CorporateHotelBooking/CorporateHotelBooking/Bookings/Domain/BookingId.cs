using CorporateHotelBooking.Application.Bookings.Domain;

namespace CorporateHotelBooking.Application.Bookings.Domain;

public record BookingId(Guid Value)
{
    public static BookingId New()
    {
        return From(Guid.NewGuid());
    }

    public static BookingId From(Guid value)
    {
        return new BookingId(value);
    }
}
