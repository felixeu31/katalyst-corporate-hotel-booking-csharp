namespace CorporateHotelBooking.Test.E2E
{
    public class HotelTest
    {
        private HotelService _hotelService;

        public HotelTest()
        {
            var hotelRepository = new InMemoryHotelRepository();
            _hotelService = new HotelService(hotelRepository);
        }

        [Fact]
        public async Task should_be_able_to_create_hotel_without_exception()
        {
            // Arrange
            int hotelId = 1;
            string hotelName = "Wesing";

            // Act
            var exception = Record.Exception(() => _hotelService.AddHotel(hotelId, hotelName));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task should_throw_exception_when_hotel_already_exists()
        {
            // Arrange
            int hotelId = 1;
            string hotelName = "Wesing";

            // Act
            _hotelService.AddHotel(hotelId, hotelName);

            // Assert
            Assert.Throws<ExistingHotelException>(() => _hotelService.AddHotel(hotelId, hotelName));
        }
    }
}