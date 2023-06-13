using FluentAssertions;
using Moq;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Hotels.UseCases;
using CorporateHotelBooking.Test.Constants;

namespace CorporateHotelBooking.Test.Unit.UseCases;

[Trait(TestTrait.Category, TestCategory.Unit)]
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