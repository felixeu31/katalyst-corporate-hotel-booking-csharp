namespace CorporateHotelBooking.Test.E2E
{
    public class HotelTest
    {
        [Fact]
        public async Task should_be_able_to_create_hotel_without_exception()
        {
            // Arrange
            var hotelService = new HotelService();
            int hotelId = 1;
            string hotelName = "Wesing";

            // Act
            var exception = Record.Exception(() => hotelService.AddHotel(hotelId, hotelName));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task should_throw_exception_when_hotel_already_exists()
        {
            // Arrange
            var hotelService = new HotelService();
            int hotelId = 1;
            string hotelName = "Wesing";

            // Act
            hotelService.AddHotel(hotelId, hotelName);

            // Assert
            Assert.Throws<ExistingHotelException>(() => hotelService.AddHotel(hotelId, hotelName));
        }
    }
}