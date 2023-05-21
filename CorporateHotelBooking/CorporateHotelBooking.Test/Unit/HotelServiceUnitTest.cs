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
    }
}
