namespace CorporateHotelBooking.Test.E2E
{
    public class HotelTest
    {
        [Fact]
        public async Task should_be_able_to_create_hotel()
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
    }

    public class HotelService
    {
        public void AddHotel(int hotelId, string hotelName)
        {
            throw new NotImplementedException();
        }
    }
}