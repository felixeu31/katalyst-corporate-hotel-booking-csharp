using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace CorporateHotelBooking.Test.E2E
{
    public  class WeatherForeCastTest
    {
        private readonly HttpClient _httpClient;

        public WeatherForeCastTest()
        {

            var testServer = new WebApplicationFactory<Program>();
            _httpClient = testServer.CreateClient();
        }

        [Fact]
        public async Task should_be_able_to_retrieve_weather_forecast()
        {
            // Act
            var response = await _httpClient.GetAsync("WeatherForecast");
            var weatherForecastResponse = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(weatherForecastResponse);
            Assert.InRange(weatherForecastResponse.Count(), 1, 5);
        }
    }
}
