using CorporateHotelBooking.Hotels.Application;
using CorporateHotelBooking.Hotels.Domain;
using FluentAssertions;
using Moq;

namespace CorporateHotelBooking.Test.Unit.UseCases
{
    public class FindHotelUseCaseTest
    {
        private readonly Mock<IHotelRepository> _hotelRepository;

        public FindHotelUseCaseTest()
        {
            _hotelRepository = new();
        }

        [Fact]
        public void should_find_hotel()
        {
            // Arrange
            Guid hotelId = Guid.NewGuid();
            string hotelName = "Westing";
            var findHotelUseCase = new FindHotelUseCase(_hotelRepository.Object);
            _hotelRepository.Setup(x => x.Get(HotelId.From(hotelId))).Returns(new Hotel(HotelId.From(hotelId), string.Empty));

            // Act
            var hotel = findHotelUseCase.Execute(hotelId);

            // Assert
            _hotelRepository.Verify(x => x.Get(HotelId.From(hotelId)), Times.Once());

        }
    }
}
