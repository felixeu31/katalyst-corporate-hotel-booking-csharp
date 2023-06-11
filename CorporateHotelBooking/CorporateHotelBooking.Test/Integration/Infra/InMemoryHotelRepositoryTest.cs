using CorporateHotelBooking.Test.Constants;
using FluentAssertions;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Data.InMemory;
using CorporateHotelBooking.Data.InMemory.Repositories;

namespace CorporateHotelBooking.Test.Integration.Infra
{
    public class InMemoryHotelRepositoryTest
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly InMemoryContext _context;

        public InMemoryHotelRepositoryTest()
        {
            _context = new InMemoryContext();
            _hotelRepository = new InMemoryHotelRepository(_context);
        }

        [Fact]
        public void should_add_hotel()
        {
            // Arrange
            HotelId hotelId = HotelId.New();
            string hotelName = "Westing";
            var newHotel = new Hotel(hotelId, hotelName);

            // Act
            _hotelRepository.Add(newHotel);

            // Assert
            Hotel? hotel = _context.Hotels[hotelId];
            hotel.Should().NotBeNull();
            hotel.HotelId.Should().Be(hotelId);
            hotel.HotelName.Should().Be(hotelName);
        }

        [Fact]
        public void should_retrieve_hotel()
        {
            // Arrange
            HotelId hotelId = HotelId.New();
            string hotelName = "Westing";
            var newHotel = new Hotel(hotelId, hotelName);
            _context.Hotels.Add(newHotel.HotelId, newHotel);

            // Act
            Hotel? hotel = _hotelRepository.Get(hotelId);

            // Assert
            hotel.Should().NotBeNull();
            hotel.HotelId.Should().Be(hotelId);
            hotel.HotelName.Should().Be(hotelName);
        }


        [Fact]
        public void should_insert_new_room_to_hotel_when_new()
        {
            // Arrange

            HotelId hotelId = HotelId.New();
            string hotelName = "Westing";
            var roomNumber = 1;
            var roomType = RoomTypes.Deluxe;
            var newHotel = new Hotel(hotelId, hotelName);
            _context.Hotels.Add(newHotel.HotelId, newHotel);

            var updatedHotel = new Hotel(hotelId, hotelName);
            updatedHotel.SetRoom(roomNumber, roomType);

            // Act
            _hotelRepository.Update(updatedHotel);

            // Assert
            Hotel hotel = _context.Hotels[hotelId];
            hotel.Should().NotBeNull();
            hotel.Rooms.Should().HaveCount(1);
            hotel.Rooms.First().RoomNumber.Should().Be(roomNumber);
            hotel.Rooms.First().RoomType.Should().Be(roomType);
        }
    }
}
