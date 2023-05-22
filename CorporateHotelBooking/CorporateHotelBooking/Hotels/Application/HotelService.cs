using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Hotels.Application;

public class HotelService
{
    private readonly HotelRepository _hotelRepository;

    public HotelService(HotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public void AddHotel(int hotelId, string hotelName)
    {
        _hotelRepository.AddHotel(hotelId, hotelName);
    }

    public void SetRoom(int hotelId, int roomNumber, string roomType)
    {
        var hotel = _hotelRepository.GetById(hotelId);

        if (hotel.Rooms.Exists(x => x.RoomNumber == roomNumber))
        {
            _hotelRepository.UpdateRoom(hotelId, roomNumber, roomType);
        }

        _hotelRepository.AddRoom(hotelId, roomNumber, roomType);
    }
}