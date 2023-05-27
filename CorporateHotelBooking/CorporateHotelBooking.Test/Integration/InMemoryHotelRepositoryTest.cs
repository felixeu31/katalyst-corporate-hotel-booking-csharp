﻿using System;
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
        private readonly IHotelRepository _hotelRepository;

        public InMemoryHotelRepositoryTest()
        {
            _hotelRepository = new InMemoryHotelRepository();
        }

        [Fact]
        public void should_retrieve_added_hotel()
        {
            // Arrange
            HotelId hotelId = HotelId.New();
            string hotelName = "Westing";

            // Act
            _hotelRepository.Add(new Hotel(hotelId, hotelName));
            Hotel hotel = _hotelRepository.Get(hotelId);

            // Assert
            hotel.Should().NotBeNull();
            hotel.HotelId.Should().Be(hotelId);
            hotel.HotelName.Should().Be(hotelName);
        }


        [Fact]
        public void should_insert_new_room_to_hotel_when_new()
        {
            // Arrange

            HotelId hotelId = HotelId.New();
            string hotelName = "Westing";
            var roomNumber = 1;
            var roomType = "Deluxe";
            var newHotel = new Hotel(hotelId, hotelName);

            // Act
            _hotelRepository.Add(newHotel);
            newHotel.SetRoom(roomNumber, roomType);
            _hotelRepository.Update(newHotel);
            Hotel hotel = _hotelRepository.Get(hotelId);

            // Assert
            hotel.Should().NotBeNull();
            hotel.Rooms.Should().HaveCount(1);
            hotel.Rooms.First().RoomNumber.Should().Be(roomNumber);
            hotel.Rooms.First().RoomType.Should().Be(roomType);
        }


        //[Fact]
        //public void should_update_hotel_room_when_existing()
        //{
        //    // Arrange
        //    
        //    HotelId hotelId = HotelId.New();
        //    string hotelName = "Westing";
        //    var roomNumber = 1;
        //    var roomType = "Deluxe";
        //    var otherRoomType = "Standard";

        //    // Act
        //    _hotelRepository.Add(hotelId, hotelName);
        //    _hotelRepository.AddRoom(hotelId, roomNumber, roomType);
        //    _hotelRepository.UpdateRoom(hotelId, roomNumber, otherRoomType);
        //    Hotel hotel = _hotelRepository.GetById(hotelId);

        //    // Assert
        //    hotel.Should().NotBeNull();
        //    hotel.Rooms.Should().HaveCount(1);
        //    hotel.Rooms.First().RoomNumber.Should().Be(roomNumber);
        //    hotel.Rooms.First().RoomType.Should().Be(otherRoomType);
        //}
    }
}