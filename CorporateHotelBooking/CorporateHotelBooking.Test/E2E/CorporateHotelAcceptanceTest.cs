using CorporateHotelBooking.Api.Controllers;
using CorporateHotelBooking.Test.ApiFactory;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using CorporateHotelBooking.Application.Hotels.UseCases;
using CorporateHotelBooking.Test.Constants;
using CorporateHotelBooking.Test.TestUtilities;

namespace CorporateHotelBooking.Test.E2E
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>dotnet watch test --project .\CorporateHotelBooking.Test\CorporateHotelBooking.Test.csproj</remarks>
    [Trait(TestTrait.Category, TestCategory.EndToEnd)]
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
            var hotelId = await GivenHotelWith(new List<RoomDto>() { new RoomDto(1, SampleData.RoomTypes.Suite) });
            var (companyId, employeeId) = await GivenEmployeeInCompany();
            await GivenEmployeeHasPolicy(employeeId, SampleData.RoomTypes.Suite);

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new { hotelId, employeeId,
                roomType = SampleData.RoomTypes.Suite,
                checkIn = DateTime.Today,
                checkOut = DateTime.Today.AddDays(7) });
            Assert.Equal(HttpStatusCode.Created, bookResponse.StatusCode);
            var booking = await bookResponse.Content.ReadFromJsonAsync<BookingDto>();

            // Assert
            booking.HotelId.Should().Be(hotelId);
            booking.BookedBy.Should().Be(employeeId);
            booking.RoomType.Should().Be(SampleData.RoomTypes.Suite);
            booking.RoomNumber.Should().Be(1);
            booking.CheckIn.Should().BeSameDateAs(DateTime.Today);
            booking.CheckOut.Should().BeSameDateAs(DateTime.Today.AddDays(7));
        }

        [Fact]
        public async Task should_return_conflict_when_trying_to_add_duplicated_hotel()
        {
            // Arrange
            var hotelId = await GivenHotelWith(new List<RoomDto>() { new RoomDto(1, SampleData.RoomTypes.Suite) });

            // Act
            var duplicatedAddHotelResponse = await _client.PostAsJsonAsync("hotels", new { HotelId = hotelId, HotelName = "Same hotel" });

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, duplicatedAddHotelResponse.StatusCode);
        }

        [Fact]
        public async Task set_room_should_return_not_found_when_hotel_does_not_exist()
        {
            // Arrange
            // Act
            var setRoomResponse = await _client.PostAsJsonAsync($"hotels/{Guid.NewGuid()}/rooms", new { RoomNumber = 1, RoomType = SampleData.RoomTypes.Suite });

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, setRoomResponse.StatusCode);

        }

        [Fact]
        public async Task find_hotel_should_return_hotel_information()
        {
            // Arrange
            var hotelName = "Westing";
            var hotelId = await GivenHotelWith(new List<RoomDto>()
            {
                new RoomDto( 1, SampleData.RoomTypes.Suite),
                new RoomDto( 2, SampleData.RoomTypes.Suite),
                new RoomDto( 3, SampleData.RoomTypes.Suite),
                new RoomDto( 4, SampleData.RoomTypes.Suite),
                new RoomDto( 5, SampleData.RoomTypes.Standard),
                new RoomDto( 6, SampleData.RoomTypes.Standard)
            });

            // Act
            var findHotelResponse = await _client.GetAsync($"hotels/{hotelId}");
            Assert.Equal(HttpStatusCode.OK, findHotelResponse.StatusCode);
            var hotelDto = await findHotelResponse.Content.ReadFromJsonAsync<HotelDto>();

            // Assert
            hotelDto.Should().NotBeNull();
            hotelDto.HotelId.Should().Be(hotelId);
            hotelDto.HotelName.Should().Be(hotelName);
            hotelDto.RoomCount.Should().HaveCount(2);
            hotelDto.RoomCount[SampleData.RoomTypes.Suite].Should().Be(4);
            hotelDto.RoomCount[SampleData.RoomTypes.Standard].Should().Be(2);
            hotelDto.Rooms.Should().HaveCount(6);
            hotelDto.Rooms.First(x => x.RoomNumber.Equals(1)).RoomType.Should().Be(SampleData.RoomTypes.Suite);
        }

        [Fact]
        public async Task should_return_conflict_when_room_is_not_offered_by_hotel()
        {
            // Arrange
            var hotelId = await GivenHotelWith(new List<RoomDto>() { new RoomDto(1, SampleData.RoomTypes.Suite) });
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
        public async Task should_return_conflict_when_booking_period_is_invalid()
        {
            // Arrange
            var hotelId = await GivenHotelWith(new List<RoomDto>() { new RoomDto(1, SampleData.RoomTypes.Suite) });
            var (companyId, employeeId) = await GivenEmployeeInCompany();

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new
            {
                hotelId,
                employeeId,
                RoomType = SampleData.RoomTypes.Suite,
                CheckIn = DateTime.Today.AddDays(1),
                CheckOut = DateTime.Today.AddDays(1)
            });

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, bookResponse.StatusCode);
        }

        [Fact]
        public async Task should_return_conflict_when_room_type_is_not_available_at_the_moment()
        {
            // Arrange
            var hotelId = await GivenHotelWith(new List<RoomDto>
            {
                new RoomDto(1, SampleData.RoomTypes.Standard),
                new RoomDto(2, SampleData.RoomTypes.Standard),
                new RoomDto(3, SampleData.RoomTypes.Deluxe)
            });
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
            var hotelId = await GivenHotelWith(new List<RoomDto>
            {
                new RoomDto(1, SampleData.RoomTypes.Standard),
                new RoomDto(2, SampleData.RoomTypes.Standard),
                new RoomDto(3, SampleData.RoomTypes.Deluxe)
            });
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
            var hotelId = await GivenHotelWith(new List<RoomDto>
            {
                new RoomDto(1, SampleData.RoomTypes.Standard),
                new RoomDto(2, SampleData.RoomTypes.Standard),
                new RoomDto(3, SampleData.RoomTypes.Deluxe)
            });
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

        [Fact]
        public async void should_be_able_to_book_a_room_when_no_policies()
        {
            // Arrange
            var hotelId = await GivenHotelWith(new List<RoomDto>() { new RoomDto(1, SampleData.RoomTypes.Suite) });
            var (companyId, employeeId) = await GivenEmployeeInCompany();

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new { hotelId, employeeId, roomType = SampleData.RoomTypes.Suite, checkIn = DateTime.Today,
                checkOut = DateTime.Today.AddDays(7) });

            // Assert
            Assert.Equal(HttpStatusCode.Created, bookResponse.StatusCode);
        }

        [Fact]
        public async void should_not_allow_an_employee_to_book_a_room_that_is_not_contained_in_the_employee_policy()
        {
            // Arrange
            var hotelId = await GivenHotelWith(new List<RoomDto>() { new RoomDto(1, SampleData.RoomTypes.Suite) });
            var (companyId, employeeId) = await GivenEmployeeInCompany();
            await GivenEmployeeHasPolicy(employeeId, SampleData.RoomTypes.Standard);

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new { hotelId, employeeId, RoomType = SampleData.RoomTypes.Suite, checkIn = DateTime.Today,
                checkOut = DateTime.Today.AddDays(7) });

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, bookResponse.StatusCode);
        }

        [Fact]
        public async void should_not_allow_an_employee_to_book_a_room_that_is_not_contained_in_the_company_policy()
        {
            // Arrange
            var hotelId = await GivenHotelWith(new List<RoomDto>() { new RoomDto(1, SampleData.RoomTypes.Suite) });
            var (companyId, employeeId) = await GivenEmployeeInCompany();
            await GivenCompanyHasPolicy(companyId, SampleData.RoomTypes.Standard);

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new { hotelId, employeeId, roomType = SampleData.RoomTypes.Suite, checkIn = DateTime.Today,
                checkOut = DateTime.Today.AddDays(7) });

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, bookResponse.StatusCode);
        }

        [Fact]
        public async void should_take_employee_policy_over_hotel_policy()
        {
            // Arrange
            var hotelId = await GivenHotelWith(new List<RoomDto>() { new RoomDto(1, SampleData.RoomTypes.Deluxe) });
            var (companyId, employeeId) = await GivenEmployeeInCompany();
            await GivenEmployeeHasPolicy(employeeId, SampleData.RoomTypes.Deluxe);
            await GivenCompanyHasPolicy(companyId, SampleData.RoomTypes.Standard);

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new { hotelId, employeeId, roomType = SampleData.RoomTypes.Deluxe, checkIn = DateTime.Today,
                checkOut = DateTime.Today.AddDays(7) });

            // Assert
            Assert.Equal(HttpStatusCode.Created, bookResponse.StatusCode);
        }

        [Fact]
        public async void should_use_last_company_policy_update_to_validate_policies()
        {
            // Arrange
            var hotelId = await GivenHotelWith(new List<RoomDto>() { new RoomDto(1, SampleData.RoomTypes.Suite) });
            var (companyId, employeeId) = await GivenEmployeeInCompany();
            await GivenCompanyHasPolicy(companyId, SampleData.RoomTypes.Standard);
            await _client.PostAsJsonAsync($"policies/company", new { CompanyId = companyId, RoomTypes = new List<string> { SampleData.RoomTypes.Deluxe } });

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new { hotelId, employeeId, roomType = SampleData.RoomTypes.Standard, checkIn = DateTime.Today,
                checkOut = DateTime.Today.AddDays(7) });

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, bookResponse.StatusCode);
        }

        [Fact]
        public async void should_use_last_employee_policy_update_to_validate_policies()
        {
            // Arrange
            var hotelId = await GivenHotelWith(new List<RoomDto>() { new RoomDto(1, SampleData.RoomTypes.Suite) });
            var (companyId, employeeId) = await GivenEmployeeInCompany();
            await GivenEmployeeHasPolicy(employeeId, SampleData.RoomTypes.Standard);
            await _client.PostAsJsonAsync($"policies/employee", new { EmployeeId = employeeId, RoomTypes = new List<string> { SampleData.RoomTypes.Deluxe } });

            // Act
            var bookResponse = await _client.PostAsJsonAsync("bookings", new { hotelId, employeeId, roomType = SampleData.RoomTypes.Standard, checkIn = DateTime.Today,
                checkOut = DateTime.Today.AddDays(7) });

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, bookResponse.StatusCode);
        }

        [Fact]
        public async void should_allow_book_when_recreated_employee_does_not_have_his_old_policies()
        {
            // Arrange
            var hotelId = await GivenHotelWith(new List<RoomDto>() { new RoomDto(1, SampleData.RoomTypes.Suite) });
            var (companyId, employeeId) = await GivenEmployeeInCompany();
            await GivenEmployeeHasPolicy(employeeId, SampleData.RoomTypes.Standard);

            // Act
            var deletedEmployeeResponse = await _client.DeleteAsync($"employees/{employeeId}");
            Assert.Equal(HttpStatusCode.OK, deletedEmployeeResponse.StatusCode);
            var recreateEmployeeResponse = await _client.PostAsJsonAsync("employees", new { CompanyId = companyId, EmployeeId = employeeId });
            Assert.Equal(HttpStatusCode.Created, recreateEmployeeResponse.StatusCode);
            var bookResponse = await _client.PostAsJsonAsync("bookings", new { hotelId, employeeId, roomType = SampleData.RoomTypes.Suite, checkIn = DateTime.Today,
                checkOut = DateTime.Today.AddDays(7) });

            // Assert
            Assert.Equal(HttpStatusCode.Created, bookResponse.StatusCode);
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

        private async Task GivenEmployeeHasPolicy(Guid employeeId, string roomType)
        {
            var setEmployeePolicyResponse = await _client.PostAsJsonAsync($"policies/employee",
                new { EmployeeId = employeeId, RoomTypes = new List<string> { roomType } });
            Assert.Equal(HttpStatusCode.Created, setEmployeePolicyResponse.StatusCode);
        }

        private async Task GivenCompanyHasPolicy(Guid companyId, string roomType)
        {
            var setEmployeePolicyResponse = await _client.PostAsJsonAsync($"policies/company",
                new { CompanyId = companyId, RoomTypes = new List<string> { roomType } });
            Assert.Equal(HttpStatusCode.Created, setEmployeePolicyResponse.StatusCode);
        }

        public record RoomDto(int RoomNumber, string RoomType);
    }
}