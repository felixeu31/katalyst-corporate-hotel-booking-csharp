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
        private readonly Mock<IHotelRepository> _hotelRepository;

        public HotelServiceUnitTest()
        {
            _hotelRepository = new();
        }

        [Fact]
        public void AddHotel_ShouldStoreHotel()
        {
            // Arrange
            Guid hotelId = Guid.NewGuid();
            string hotelName = "Westing";
            var hotelService = new HotelService(_hotelRepository.Object);

            // Act
            hotelService.AddHotel(hotelId, hotelName);

            // Assert
            _hotelRepository.Verify(x => x.Add(It.IsAny<Hotel>()), Times.Once());
        }

        [Fact]
        public void SetRoom_WhenNew_ShouldAddRoom()
        {
            // Arrange
            Guid hotelId = Guid.NewGuid();
            string hotelName = "Westing";
            var roomNumber = 1;
            var roomType = "Deluxe";
            _hotelRepository
                .Setup(repository => repository.Get(HotelId.From(hotelId)))
                    .Returns(new Hotel(HotelId.From(hotelId), hotelName));
            var hotelService = new HotelService(_hotelRepository.Object);
            hotelService.AddHotel(hotelId, hotelName);

            // Act
            hotelService.SetRoom(hotelId, roomNumber, roomType);

            // Assert
            _hotelRepository.Verify(x => x.Add(It.IsAny<Hotel>()), Times.Once());
        }


        [Fact]
        public void SetRoom_WhenExisting_ShouldUpdateRoom()
        {
            // Arrange
            Guid hotelId = Guid.NewGuid();
            string hotelName = "Westing";
            var roomNumber = 1;
            var roomType = "Deluxe";
            var otherRoomType = "Standard";
            var hotel = new Hotel(HotelId.From(hotelId), hotelName);
            hotel.AddRoom(roomNumber, roomType);
            _hotelRepository.Setup(repository => repository.Get(HotelId.From(hotelId))).Returns(hotel);
            var hotelService = new HotelService(_hotelRepository.Object);
            hotelService.AddHotel(hotelId, hotelName);

            // Act
            hotelService.SetRoom(hotelId, roomNumber, roomType);
            hotelService.SetRoom(hotelId, roomNumber, otherRoomType);

            // Assert
            _hotelRepository.Verify(x => x.Update(It.IsAny<Hotel>()));
        }
    }
}
