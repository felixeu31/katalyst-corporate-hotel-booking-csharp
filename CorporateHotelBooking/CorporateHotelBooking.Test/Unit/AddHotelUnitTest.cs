using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace CorporateHotelBooking.Test.Unit
{
    public class AddHotelUnitTest
    {
        [Fact]
        public void AddHotel_ShouldAddHotelToRepository()
        {
            // Arrange
            var hotelRepository = new Mock<HotelRepository>();
            var hotelService = new HotelService(hotelRepository.Object);

            // Act
            hotelService.AddHotel(1, "Westing");

            // Assert
            hotelRepository.Verify(x => x.AddHotel(1, "Westing"), Times.Once);
        }
    }
}
