using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorporateHotelBooking.Hotels.Domain;
using CorporateHotelBooking.Hotels.Infrastructure;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Integration
{
    public class InMemoryHotelRepositoryTest
    {
        private readonly HotelRepository _hotelRepository;

        public InMemoryHotelRepositoryTest()
        {
            _hotelRepository = new InMemoryHotelRepository();
        }

        [Fact]
        public void should_retrieve_added_hotel()
        {
            // Arrange
            int hotelId = 1;
            string hotelName = "Westing";

            // Act
            _hotelRepository.AddHotel(hotelId, hotelName);
            Hotel hotel = _hotelRepository.GetById(hotelId);
            
            // Assert
            hotel.Should().NotBeNull();
            hotel.HotelId.Should().Be(1);
            hotel.HotelName.Should().Be(hotelName);
        }
    }
}
