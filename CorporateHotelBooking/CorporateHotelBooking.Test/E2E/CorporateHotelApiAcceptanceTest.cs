using Castle.Core.Resource;
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
    public class CorporateHotelApiAcceptanceTest : IClassFixture<CorporateHotelApiFactory>
    {
        private readonly HttpClient _client;

        public CorporateHotelApiAcceptanceTest(CorporateHotelApiFactory apiFactory)
        {
            _client = apiFactory.CreateClient();
        }

        [Fact]
        public async Task an_existing_room_in_hotel_should_be_booked()
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
            var booking = await bookResponse.Content.ReadFromJsonAsync<Booking>();

            // Assert
            booking.HotelId.Should().Be(HotelId.From(hotelId));
            booking.BookedBy.Should().Be(EmployeeId.From(employeeId));
            booking.RoomType.Should().Be(roomType);
            booking.RoomNumber.Should().Be(roomNumber);
            booking.CheckIn.Should().BeSameDateAs(checkIn);
            booking.CheckOut.Should().BeSameDateAs(checkOut);
        }

    }
}