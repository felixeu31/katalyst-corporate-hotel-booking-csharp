using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Hotels.Application;

public class HotelService
{
    private readonly IHotelRepository _hotelRepository;

    public HotelService(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public void AddHotel(Guid hotelId, string hotelName)
    {
        _hotelRepository.Add(new Hotel(HotelId.From(hotelId), hotelName));
    }

    public void SetRoom(Guid hotelId, int roomNumber, string roomType)
    {
        var hotel = _hotelRepository.Get(HotelId.From(hotelId));

        hotel.SetRoom(roomNumber, roomType);

        _hotelRepository.Update(hotel);
    }
}