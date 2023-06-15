using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Data.Sql.Repositories;
using CorporateHotelBooking.Data.Sql;
using CorporateHotelBooking.Test.Fixtures;
using CorporateHotelBooking.Test.Constants;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Data.Sql.DataModel;
using CorporateHotelBooking.Test.TestUtilities;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Integration.SqlDataBase;

[Collection(nameof(LocalDbTestFixtureCollection))]
[Trait(TestTrait.Category, TestCategory.Integration)]
public class SqlBookingRepositoryTest
{
    private readonly LocalDbTestFixture _fixture;
    private readonly IBookingRepository _bookingRepository;
    public SqlBookingRepositoryTest(LocalDbTestFixture fixture)
    {
        _fixture = fixture;
        _bookingRepository = new SqlBookingRepository(new CorporateHotelDbContext(fixture.DbContextOptions));
    }

    [Fact]
    public void should_add_booking()
    {
        // Arrange
        HotelId hotelId = HotelId.New();
        int roomNumber = 1;
        EmployeeId employeeId = EmployeeId.New();
        string roomType = SampleData.RoomTypes.Deluxe;
        DateTime checkIn = DateTime.Today.AddDays(1);
        DateTime chekout = DateTime.Today.AddDays(2);
        var newBooking = new Booking(roomNumber, hotelId, employeeId, roomType, checkIn, chekout);

        // Act
        _bookingRepository.Add(newBooking);

        // Assert
        using var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        BookingData? booking = context.Bookings.Find(newBooking.BookingId.Value);
        booking.Should().NotBeNull();
        booking.RoomNumber.Should().Be(1);
        booking.HotelId.Should().Be(hotelId.Value);
        booking.BookedBy.Should().Be(employeeId.Value);
        booking.RoomType.Should().Be(SampleData.RoomTypes.Deluxe);
        booking.CheckIn.Should().BeSameDateAs(DateTime.Today.AddDays(1));
        booking.CheckOut.Should().BeSameDateAs(DateTime.Today.AddDays(2));
    }
}