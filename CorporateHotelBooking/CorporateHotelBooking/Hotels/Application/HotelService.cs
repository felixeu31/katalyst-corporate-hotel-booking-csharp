using CorporateHotelBooking.Hotels.Domain;

namespace CorporateHotelBooking.Hotels.Application;

public class HotelService
{
    private readonly IHotelRepository _hotelRepository;

    public HotelService(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public void AddHotel(int hotelId, string hotelName)
    {
        _hotelRepository.Add(new Hotel(hotelId, hotelName));
    }

    public void SetRoom(int hotelId, int roomNumber, string roomType)
    {
        var hotel = _hotelRepository.GetById(hotelId); // Consulta 1

        hotel.SetRoom(roomNumber, roomType);

        _hotelRepository.Update(hotel);
    }
}