using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Data.Sql;
using CorporateHotelBooking.Data.Sql.DataModel;
using CorporateHotelBooking.Data.Sql.Repositories;
using CorporateHotelBooking.Test.Constants;
using CorporateHotelBooking.Test.Fixtures;
using CorporateHotelBooking.Test.TestUtilities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CorporateHotelBooking.Test.Integration.SqlDataBase;

[Collection(nameof(LocalDbTestFixtureCollection))]
[Trait(TestTrait.Category, TestCategory.Integration)]
public class SqlHotelRepositoryTest
{
    private readonly LocalDbTestFixture _fixture;
    private readonly IHotelRepository _hotelRepository;

    public SqlHotelRepositoryTest(LocalDbTestFixture fixture)
    {
        _fixture = fixture;
        _hotelRepository = new SqlHotelRepository(new CorporateHotelDbContext(fixture.DbContextOptions));
    }


    [Fact]
    public void should_add_hotel()
    {
        // Arrange
        HotelId hotelId = HotelId.New();
        string hotelName = "Westing";
        var newHotel = new Hotel(hotelId, hotelName);

        // Act
        _hotelRepository.Add(newHotel);

        // Assert
        using var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        HotelData? hotel = context.Hotels.Find(newHotel.HotelId.Value);
        hotel.Should().NotBeNull();
        hotel.HotelId.Should().Be(hotelId.Value);
        hotel.HotelName.Should().Be(hotelName);
    }
    
    [Fact]
    public void should_get_hotel()
    {
        // Arrange
        using var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        var hotelData = new HotelData
        {
            HotelId = Guid.NewGuid(),
            HotelName = "Westing"
        };
        context.Hotels.Add(hotelData);
        context.SaveChanges();

        // Act
        var hotel = _hotelRepository.Get(HotelId.From(hotelData.HotelId));

        // Assert
        hotel.Should().NotBeNull();
        hotel.HotelId.Should().Be(HotelId.From(hotelData.HotelId));
        hotel.HotelName.Should().Be(hotelData.HotelName);
    }


    [Fact]
    public void should_update_hotel()
    {
        // Arrange
        using var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        var hotelData = new HotelData
        {
            HotelId = Guid.NewGuid(),
            HotelName = "Westing"
        };
        context.Hotels.Add(hotelData);
        context.SaveChanges();

        var updatedHotel = new Hotel(HotelId.From(hotelData.HotelId), hotelData.HotelName);
        updatedHotel.SetRoom(1, SampleData.RoomTypes.Junior);

        // Act
        _hotelRepository.Update(updatedHotel);

        // Assert
        HotelData? hotel = context.Hotels
            .Where(x => x.HotelId == updatedHotel.HotelId.Value)
            .Include(x => x.Rooms)
            .SingleOrDefault();
        hotel.Should().NotBeNull();
        hotel.Rooms.Should().HaveCount(1);
        hotel.Rooms.First().RoomNumber.Should().Be(1);
        hotel.Rooms.First().RoomType.Should().Be(SampleData.RoomTypes.Junior);
    }
}