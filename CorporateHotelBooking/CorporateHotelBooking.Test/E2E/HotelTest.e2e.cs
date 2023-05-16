using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace CorporateHotelBooking.Test.E2E
{
    public class HotelTest
    {
        private readonly HttpClient _httpClient;

        public HotelTest()
        {

            var testServer = new WebApplicationFactory<Program>();
            _httpClient = testServer.CreateClient();
        }

        [Fact]
        public async Task should_be_able_to_create_hotel()
        {
            // Act
            var response = await _httpClient.PostAsJsonAsync("hotels", new
            {
                hotelId = "1",
                hotelName = "Westing"
            });

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}