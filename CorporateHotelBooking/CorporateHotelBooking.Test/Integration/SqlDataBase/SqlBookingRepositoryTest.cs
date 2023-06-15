using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Data.Sql.Repositories;
using CorporateHotelBooking.Data.Sql;
using CorporateHotelBooking.Test.Constants;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Data.Sql.DataModel;
using CorporateHotelBooking.Test.Fixtures;
using CorporateHotelBooking.Test.Fixtures.DataBase;
using CorporateHotelBooking.Test.TestUtilities;
using FluentAssertions;
using CorporateHotelBooking.Data.InMemory;

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

    [Fact]
    public void should_get_booking()
    {
        // Arrange
        using var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        var bookingData = new BookingData
        {
            BookingId = BookingId.New().Value,
            HotelId = HotelId.New().Value,
            BookedBy = EmployeeId.New().Value,
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(2),
            RoomType = SampleData.RoomTypes.Junior,
            RoomNumber = 1,


        };
        context.Bookings.Add(bookingData);
        context.SaveChanges();

        // Act
        var booking = _bookingRepository.Get(BookingId.From(bookingData.BookingId));

        // Assert
        booking.Should().NotBeNull();
        booking.BookingId.Should().Be(BookingId.From(bookingData.BookingId));
        booking.RoomNumber.Should().Be(bookingData.RoomNumber);
        booking.HotelId.Should().Be(HotelId.From(bookingData.HotelId));
        booking.BookedBy.Should().Be(EmployeeId.From(bookingData.BookedBy));
        booking.RoomType.Should().Be(bookingData.RoomType);
        booking.CheckIn.Should().BeSameDateAs(bookingData.CheckIn);
        booking.CheckOut.Should().BeSameDateAs(bookingData.CheckOut);
    }

    [Fact]
    public void should_get_bookings_by_hotel()
    {
        // Arrange
        using var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        var hotelId = HotelId.New();
        var hotelId2 = HotelId.New();
        context.Bookings.Add(new BookingData
        {
            BookingId = BookingId.New().Value,
            HotelId = hotelId.Value,
            BookedBy = EmployeeId.New().Value,
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(2),
            RoomType = SampleData.RoomTypes.Junior,
            RoomNumber = 1
        });
        context.Bookings.Add(new BookingData
        {
            BookingId = BookingId.New().Value,
            HotelId = hotelId.Value,
            BookedBy = EmployeeId.New().Value,
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(2),
            RoomType = SampleData.RoomTypes.Junior,
            RoomNumber = 1
        });
        context.Bookings.Add(new BookingData
        {
            BookingId = BookingId.New().Value,
            HotelId = hotelId2.Value,
            BookedBy = EmployeeId.New().Value,
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(2),
            RoomType = SampleData.RoomTypes.Junior,
            RoomNumber = 1
        });
        context.SaveChanges();

        // Act
        var bookings = _bookingRepository.GetBookingsBy(hotelId);

        // Assert
        bookings.Should().NotBeNull();
        bookings.Should().HaveCount(2);
        bookings.Should().AllSatisfy(x => x.HotelId.Equals(hotelId));
    }


    [Fact]
    public void should_remove_bookings_by_employee()
    {
        // Arrange
        using var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        var hotelId = HotelId.New();
        var hotelId2 = HotelId.New();
        var employeeId = EmployeeId.New();
        var employeeId2 = EmployeeId.New();
        context.Bookings.Add(new BookingData
        {
            BookingId = BookingId.New().Value,
            HotelId = hotelId.Value,
            BookedBy = employeeId.Value,
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(2),
            RoomType = SampleData.RoomTypes.Junior,
            RoomNumber = 1
        });
        context.Bookings.Add(new BookingData
        {
            BookingId = BookingId.New().Value,
            HotelId = hotelId.Value,
            BookedBy = employeeId.Value,
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(2),
            RoomType = SampleData.RoomTypes.Junior,
            RoomNumber = 1
        });
        context.Bookings.Add(new BookingData
        {
            BookingId = BookingId.New().Value,
            HotelId = hotelId2.Value,
            BookedBy = employeeId2.Value,
            CheckIn = DateTime.Today,
            CheckOut = DateTime.Today.AddDays(2),
            RoomType = SampleData.RoomTypes.Junior,
            RoomNumber = 1
        });
        context.SaveChanges();

        // Act
        _bookingRepository.DeleteEmployeeBookings(employeeId);

        // Assert
        var employeeBookings =
            context.Bookings.Where(x => x.BookedBy.Equals(employeeId.Value));
        employeeBookings.Should().HaveCount(0);

        var employee2Bookings =
            context.Bookings.Where(x => x.BookedBy.Equals(employeeId2.Value));
        employee2Bookings.Should().HaveCount(1);
    }

}