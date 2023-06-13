using Moq;
using FluentAssertions;
using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Bookings.Domain.Exceptions;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Application.Bookings.UseCases;
using CorporateHotelBooking.Application.Policies.UseCases;
using CorporateHotelBooking.Test.TestUtilities;
using CorporateHotelBooking.Test.Constants;

namespace CorporateHotelBooking.Test.Unit.UseCases;

[Trait(TestTrait.Category, TestCategory.Unit)]
public class BookUseCaseTest
{
    private readonly Mock<IBookingRepository> _bookingRepository;
    private readonly Mock<IIsBookingAllowedUseCase> _isBookingAllowedUseCase;
    private readonly Mock<IHotelRepository> _hotelRepository;
    private BookUseCase _bookUseCase;

    public BookUseCaseTest()
    {
        _bookingRepository = new();
        _isBookingAllowedUseCase = new();
        _hotelRepository = new();
        var hotel = new Hotel(HotelId.New(), "Westing");
        hotel.SetRoom(1, SampleData.RoomTypes.Deluxe);
        _hotelRepository.Setup(x => x.Get(It.IsAny<HotelId>())).Returns(hotel);
        _bookUseCase = new BookUseCase(_bookingRepository.Object, _isBookingAllowedUseCase.Object, _hotelRepository.Object);
    }

    [Fact]
    public void AddBooking_ShouldStoreBooking()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var roomType = SampleData.RoomTypes.Deluxe;
        var checkIn = DateTime.Today.AddDays(1);
        var chekout = DateTime.Today.AddDays(2);
        _isBookingAllowedUseCase.Setup(x => x.Execute(employeeId, roomType)).Returns(true);

        // Act
        _bookUseCase.Execute(hotelId, employeeId, roomType, checkIn, chekout);

        // Assert
        _bookingRepository.Verify(x => x.Add(It.IsAny<Booking>()), Times.Once());

    }

    [Fact]
    public void AddBooking_WhenPolicyIsNotContained_ShouldThrowEmployeePolicyException()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var roomType = SampleData.RoomTypes.Deluxe;
        var checkIn = DateTime.Today.AddDays(1);
        var chekout = DateTime.Today.AddDays(2);
        _isBookingAllowedUseCase.Setup(x => x.Execute(employeeId, roomType)).Returns(false);
            
        // Act
        Action action = () => _bookUseCase.Execute(hotelId, employeeId, roomType, checkIn, chekout);

        // Assert
        action.Should().Throw<EmployeeBookingPolicyException>();
    }


    [Fact]
    public void AddBooking_WhenRoomIsNotAvailable_ShouldThrowException()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var roomType = SampleData.RoomTypes.Deluxe;
        var checkIn = DateTime.Today.AddDays(3);
        var chekout = DateTime.Today.AddDays(10);
        _isBookingAllowedUseCase.Setup(x => x.Execute(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
        _bookingRepository.Setup(x => x.GetBookingsBy(It.IsAny<HotelId>())).Returns(new List<Booking>()
        {
            new Booking(1,HotelId.From(hotelId),  EmployeeId.From(employeeId), SampleData.RoomTypes.Deluxe, DateTime.Today.AddDays(-5), DateTime.Today.AddDays(5))
        });

        // Act
        Action action = () => _bookUseCase.Execute(hotelId, employeeId, roomType, checkIn, chekout);

        // Assert
        action.Should().Throw<RoomTypeNotAvailableException>();
    }

}