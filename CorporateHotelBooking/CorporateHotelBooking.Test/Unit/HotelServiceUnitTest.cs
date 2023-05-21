using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace CorporateHotelBooking.Test.Unit
{
    public class HotelServiceUnitTest
    {
        [Fact]
        public void AddHotel_ShouldStoreHotel()
        {
            // Arrange
            int hotelId = 1;
            string hotelName = "Westing";
            Mock<HotelRepository> hotelRepository = new();
            HotelService hotelService = new HotelService(hotelRepository.Object);

            // Act
            hotelService.AddHotel(hotelId, hotelName);

            // Assert
            hotelRepository.Verify(x => x.AddHotel(hotelId, hotelName), Times.Once());
        }
    }
}
