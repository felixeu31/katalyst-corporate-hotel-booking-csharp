using CorporateHotelBooking.Api.Controllers;
using CorporateHotelBooking.Test.ApiFactory;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using CorporateHotelBooking.Test.Constants;
using CorporateHotelBooking.Application.Hotels.UseCases;

namespace CorporateHotelBooking.Test.E2E
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>dotnet watch test --project .\CorporateHotelBooking.Test\CorporateHotelBooking.Test.csproj</remarks>
    public class CorporateHotelAcceptanceTest : IClassFixture<CorporateHotelApiFactory>
    {
        private readonly HttpClient _client;

        public CorporateHotelAcceptanceTest(CorporateHotelApiFactory apiFactory)
        {
            _client = apiFactory.CreateClient();
        }

        [Fact]
        public async Task should_be_able_to_book_an_available_room_in_hotel()
        {
            // Arrange
            var companyId = Guid.NewGuid();
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
            var addEmployeeResponse = await _client.PostAsJsonAsync("employees", new { CompanyId = companyId, EmployeeId = employeeId });
            Assert.Equal(HttpStatusCode.Created, addEmployeeResponse.StatusCode);
            var setEmployeePolicyResponse = await _client.PostAsJsonAsync($"policies/employee", new { EmployeeId = employeeId, RoomTypes = new List<string> { roomType } });
            Assert.Equal(HttpStatusCode.Created, setEmployeePolicyResponse.StatusCode);

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new { hotelId, employeeId, roomType, checkIn, checkOut });
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
        public async Task should_return_conflict_when_trying_to_add_duplicated_hotel()
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

        [Fact]
        public async Task find_hotel_should_return_hotel_information()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var hotelName = "Westing";
            var roomType1 = "Suite";
            var roomType2 = SampleData.RoomTypes.Standard;
            var room1 = new { RoomNumber = 1, RoomType = roomType1 };
            var room2 = new { RoomNumber = 2, RoomType = roomType1 };
            var room3 = new { RoomNumber = 3, RoomType = roomType1 };
            var room4 = new { RoomNumber = 4, RoomType = roomType1 };
            var room5 = new { RoomNumber = 5, RoomType = roomType2 };
            var room6 = new { RoomNumber = 6, RoomType = roomType2 };

            await _client.PostAsJsonAsync("hotels", new { HotelId = hotelId, HotelName = hotelName });
            await _client.PostAsJsonAsync($"hotels/{hotelId}/rooms", room1);
            await _client.PostAsJsonAsync($"hotels/{hotelId}/rooms", room2);
            await _client.PostAsJsonAsync($"hotels/{hotelId}/rooms", room3);
            await _client.PostAsJsonAsync($"hotels/{hotelId}/rooms", room4);
            await _client.PostAsJsonAsync($"hotels/{hotelId}/rooms", room5);
            await _client.PostAsJsonAsync($"hotels/{hotelId}/rooms", room6);

            // Act
            var findHotelResponse = await _client.GetAsync($"hotels/{hotelId}");
            Assert.Equal(HttpStatusCode.OK, findHotelResponse.StatusCode);
            var hotelDto = await findHotelResponse.Content.ReadFromJsonAsync<HotelDto>();

            // Assert
            hotelDto.Should().NotBeNull();
            hotelDto.HotelId.Should().Be(hotelId);
            hotelDto.HotelName.Should().Be(hotelName);
            hotelDto.RoomCount.Should().HaveCount(2);
            hotelDto.RoomCount[roomType1].Should().Be(4);
            hotelDto.RoomCount[roomType2].Should().Be(2);
            hotelDto.Rooms.Should().HaveCount(6);
            hotelDto.Rooms.First(x => x.RoomNumber.Equals(room1.RoomNumber)).RoomType.Should().Be(room1.RoomType);
        }

        [Fact]
        public async Task should_return_conflict_when_room_is_not_offered_by_hotel()
        {
            // Arrange
            var hotelId = await GivenHotelWith(new List<RoomDto>() { new RoomDto(1, "Suite") });
            var (companyId, employeeId) = await GivenEmployeeInCompany();

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new
            {
                hotelId,
                employeeId,
                RoomType = "Presidential",
                CheckIn = DateTime.Today.AddDays(1),
                CheckOut = DateTime.Today.AddDays(2)
            });

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, bookResponse.StatusCode);
        }

        [Fact]
        public async Task should_return_conflict_when_room_type_is_not_available_at_the_moment()
        {
            // Arrange
            var rooms = new List<RoomDto>
            {
                new RoomDto(1, SampleData.RoomTypes.Standard),
                new RoomDto(2, SampleData.RoomTypes.Standard),
                new RoomDto(3, SampleData.RoomTypes.Deluxe)
            };
            var hotelId = await GivenHotelWith(rooms);
            var (companyId, employeeId) = await GivenEmployeeInCompany();
            await _client.PostAsJsonAsync("bookings", new
            {
                hotelId,
                employeeId,
                RoomType = SampleData.RoomTypes.Deluxe,
                CheckIn = DateTime.Today.AddDays(-5),
                CheckOut = DateTime.Today.AddDays(5)
            });

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new
            {
                hotelId,
                employeeId,
                RoomType = SampleData.RoomTypes.Deluxe,
                CheckIn = DateTime.Today.AddDays(3),
                CheckOut = DateTime.Today.AddDays(10)
            });

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, bookResponse.StatusCode);
        }
        
        [Fact]
        public async Task should_be_able_to_book_room_when_old_bookings_from_deleted_employee_have_been_removed()
        {
            // Arrange
            var rooms = new List<RoomDto>
            {
                new RoomDto(1, SampleData.RoomTypes.Standard),
                new RoomDto(2, SampleData.RoomTypes.Standard),
                new RoomDto(3, SampleData.RoomTypes.Deluxe)
            };
            var hotelId = await GivenHotelWith(rooms);
            var (companyId, employeeId) = await GivenEmployeeInCompany();
            await _client.PostAsJsonAsync("bookings", new
            {
                hotelId,
                EmployeeId = employeeId,
                RoomType = SampleData.RoomTypes.Deluxe,
                CheckIn = DateTime.Today.AddDays(-5),
                CheckOut = DateTime.Today.AddDays(5)
            });

            var deletedEmployeeResponse = await _client.DeleteAsync($"employees/{employeeId}");
            Assert.Equal(HttpStatusCode.OK, deletedEmployeeResponse.StatusCode);

            var (companyId2, employeeId2) = await GivenEmployeeInCompany();

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new
            {
                hotelId,
                EmployeeId = employeeId2,
                RoomType = SampleData.RoomTypes.Deluxe,
                CheckIn = DateTime.Today.AddDays(3),
                CheckOut = DateTime.Today.AddDays(10)
            });

            // Assert
            Assert.Equal(HttpStatusCode.Created, bookResponse.StatusCode);
        }

        [Fact]
        public async Task should_return_not_found_when_employee_has_been_deleted()
        {
            // Arrange
            var rooms = new List<RoomDto>
            {
                new RoomDto(1, SampleData.RoomTypes.Standard),
                new RoomDto(2, SampleData.RoomTypes.Standard),
                new RoomDto(3, SampleData.RoomTypes.Deluxe)
            };
            var hotelId = await GivenHotelWith(rooms);
            var (companyId, employeeId) = await GivenEmployeeInCompany();
            var deletedEmployeeResponse = await _client.DeleteAsync($"employees/{employeeId}");
            Assert.Equal(HttpStatusCode.OK, deletedEmployeeResponse.StatusCode);

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new
            {
                hotelId,
                EmployeeId = employeeId,
                RoomType = SampleData.RoomTypes.Deluxe,
                CheckIn = DateTime.Today.AddDays(3),
                CheckOut = DateTime.Today.AddDays(10)
            });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, bookResponse.StatusCode);
        }


        private async Task<Guid> GivenHotelWith(List<RoomDto> rooms)
        {
            var hotelId = Guid.NewGuid();
            var hotelName = "Westing";

            var addHotelResponse = await _client.PostAsJsonAsync("hotels", new { HotelId = hotelId, HotelName = hotelName });
            Assert.Equal(HttpStatusCode.Created, addHotelResponse.StatusCode);

            foreach (var room in rooms)
            {
                var setRoomResponse = await _client.PostAsJsonAsync($"hotels/{hotelId}/rooms", new { room.RoomNumber, room.RoomType });
                Assert.Equal(HttpStatusCode.OK, setRoomResponse.StatusCode);
            }

            return hotelId;
        }

        private async Task<(Guid, Guid)> GivenEmployeeInCompany()
        {
            var companyId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();

            var addEmployeeResponse = await _client.PostAsJsonAsync("employees", new { CompanyId = companyId, EmployeeId = employeeId });
            Assert.Equal(HttpStatusCode.Created, addEmployeeResponse.StatusCode);

            return (companyId, employeeId);

        }

        public record RoomDto(int RoomNumber, string RoomType);
    }
}