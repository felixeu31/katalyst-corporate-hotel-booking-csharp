using CorporateHotelBooking.Test.ApiFactory;
using System.Net;
using System.Net.Http.Json;

namespace CorporateHotelBooking.Test.E2E;

public class BookingPoliceAcceptanceTest : IClassFixture<CorporateHotelApiFactory>
{
    private readonly HttpClient _client;
    public BookingPoliceAcceptanceTest(CorporateHotelApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async void should_not_allow_an_employee_to_book_a_room_that_is_not_contained_in_his_policy_USER_JOURNEY()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var hotelId = Guid.NewGuid();
        var hotelName = "Westing";
        var roomType = "Suite";
        var roomNumber = 1;
        var checkIn = DateTime.Today;
        var checkOut = DateTime.Today.AddDays(7);

        var addHotelResponse = await _client.PostAsJsonAsync("hotels", new { HotelId = hotelId, HotelName = hotelName });
        Assert.Equal(HttpStatusCode.Created, addHotelResponse.StatusCode);
        var setRoomResponse = await _client.PostAsJsonAsync($"hotels/{hotelId}/rooms", new { RoomNumber = roomNumber, RoomType = roomType });
        Assert.Equal(HttpStatusCode.OK, setRoomResponse.StatusCode);
        var setEmployeePolicyResponse = await _client.PostAsJsonAsync($"policies/employee", new { EmployeeId = employeeId, RoomTypes = new List<string>{"Standard"} });
        Assert.Equal(HttpStatusCode.Created, setEmployeePolicyResponse.StatusCode);

        // Act
        var bookResponse = await _client.PostAsJsonAsync("bookings", new { roomNumber, hotelId, employeeId, roomType, checkIn, checkOut });

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, bookResponse.StatusCode);
    }

    [Fact]
    public async void should_not_allow_an_employee_to_book_a_room_that_is_not_contained_in_his_policy_DATA_SET_UP()
    {
        // Arrange

        // Act

        // Assert
    }

}