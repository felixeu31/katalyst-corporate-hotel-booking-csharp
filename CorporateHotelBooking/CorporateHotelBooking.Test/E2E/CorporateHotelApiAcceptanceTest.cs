using Castle.Core.Resource;
using CorporateHotelBooking.Api.Controllers;
using CorporateHotelBooking.Bookings.Application;
using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Bookings.Infra;
using CorporateHotelBooking.Employees.Application;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Employees.Infra;
using CorporateHotelBooking.Hotels.Application;
using CorporateHotelBooking.Hotels.Domain;
using CorporateHotelBooking.Hotels.Infrastructure;
using CorporateHotelBooking.Test.ApiFactory;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace CorporateHotelBooking.Test.E2E
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>dotnet watch test --project .\CorporateHotelBooking.Test\CorporateHotelBooking.Test.csproj</remarks>
    public class CorporateHotelApiAcceptanceTest : IClassFixture<CorporateHotelApiFactory>
    {
        private readonly HttpClient _client;

        public CorporateHotelApiAcceptanceTest(CorporateHotelApiFactory apiFactory)
        {
            _client = apiFactory.CreateClient();
        }

        [Fact]
        public async Task should_book_an_existing_room_in_hotel()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var hotelId = Guid.NewGuid();
            var hotelName = "Westing";
            var roomType = "Suite";
            var roomNumber = 1;
            var checkIn = DateTime.Today;
            var checkOut = DateTime.Today.AddDays(7);

            var addHotelResponse = await _client.PostAsJsonAsync("hotels", new { HotelId = hotelId, HotelName = hotelName});
            Assert.Equal(HttpStatusCode.Created, addHotelResponse.StatusCode);
            var setRoomResponse = await _client.PostAsJsonAsync($"hotels/{hotelId}/rooms", new { RoomNumber = roomNumber, RoomType = roomType });
            Assert.Equal(HttpStatusCode.OK, setRoomResponse.StatusCode);

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new { roomNumber, hotelId, employeeId, roomType, checkIn, checkOut });
            Assert.Equal(HttpStatusCode.Created, bookResponse.StatusCode);
            var booking = await bookResponse.Content.ReadFromJsonAsync<BookingDto>();

            // Assert
            booking.HotelId.Should().Be(hotelId);
            booking.BookedBy.Should().Be(employeeId);
            booking.RoomType.Should().Be(roomType);
            booking.RoomNumber.Should().Be(roomNumber);
            booking.CheckIn.Should().BeSameDateAs(checkIn);
            booking.CheckOut.Should().BeSameDateAs(checkOut);
        }

        [Fact]
        public async Task add_hotel_should_return_conflict_when_trying_to_add_duplicated_hotel()
        {
            // Arrange
            var hotelId = Guid.NewGuid();

            var addHotelResponse = await _client.PostAsJsonAsync("hotels", new { HotelId = hotelId, HotelName = "Hotel 1" });
            Assert.Equal(HttpStatusCode.Created, addHotelResponse.StatusCode);

            // Act
            var duplicatedAddHotelResponse = await _client.PostAsJsonAsync("hotels", new { HotelId = hotelId, HotelName = "Same hotel" });

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, duplicatedAddHotelResponse.StatusCode);
        }

        [Fact]
        public async Task set_room_should_return_not_found_when_hotel_does_not_exist()
        {
            // Arrange
            var roomType = "Suite";
            var roomNumber = 1;

            // Act
            var setRoomResponse = await _client.PostAsJsonAsync($"hotels/{Guid.NewGuid()}/rooms", new { RoomNumber = roomNumber, RoomType = roomType });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, setRoomResponse.StatusCode);

        }

    }
}