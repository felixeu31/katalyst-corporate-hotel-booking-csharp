using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorporateHotelBooking.Hotels.Application;
using CorporateHotelBooking.Hotels.Domain;
using Moq;

namespace CorporateHotelBooking.Test.Unit
{
    public class HotelServiceUnitTest
    {
        private readonly Mock<HotelRepository> _hotelRepository;
        private readonly HotelService _hotelService;

        public HotelServiceUnitTest()
        {
            _hotelRepository = new();
            _hotelService = new HotelService(_hotelRepository.Object);
        }

        [Fact]
        public void AddHotel_ShouldStoreHotel()
        {
            // Arrange
            int hotelId = 1;
            string hotelName = "Westing";

            // Act
            _hotelService.AddHotel(hotelId, hotelName);

            // Assert
            _hotelRepository.Verify(x => x.AddHotel(hotelId, hotelName), Times.Once());
        }

        [Fact]
        public void SetRoom_WhenNew_ShouldAddRoom()
        {
            // Arrange
            int hotelId = 1;
            string hotelName = "Westing";
            var roomNumber = 1;
            var roomType = "Deluxe";
            _hotelRepository.Setup(repository => repository.GetById(hotelId)).Returns(new Hotel(hotelId, hotelName));

            // Act
            _hotelService.SetRoom(hotelId, roomNumber, roomType);

            // Assert
            _hotelRepository.Verify(x => x.AddRoom(hotelId, roomNumber, roomType), Times.Once());
        }


        [Fact]
        public void SetRoom_WhenExisting_ShouldUpdateRoom()
        {
            // Arrange
            int hotelId = 1;
            string hotelName = "Westing";
            var roomNumber = 1;
            var roomType = "Deluxe";
            var otherRoomType = "Standard";
            var hotel = new Hotel(hotelId, hotelName);
            hotel.AddRoom(new Room(roomNumber, roomType));
            _hotelRepository.Setup(repository => repository.GetById(hotelId)).Returns(hotel);

            // Act
            _hotelService.SetRoom(hotelId, roomNumber, roomType);
            _hotelService.SetRoom(hotelId, roomNumber, otherRoomType);

            // Assert
            _hotelRepository.Verify(x => x.UpdateRoom(hotelId, roomNumber, roomType), Times.Once());
        }
    }
}
