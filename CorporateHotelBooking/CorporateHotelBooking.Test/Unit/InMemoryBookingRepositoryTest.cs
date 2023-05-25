﻿using CorporateHotelBooking.Bookings.Domain;
using CorporateHotelBooking.Bookings.Infra;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Unit
{
    public class InMemoryBookingRepositoryTest
    {
        private readonly IBookingRepository _bookingRepository;

        public InMemoryBookingRepositoryTest()
        {
            _bookingRepository = new InMemoryBookingRepository();
        }

        [Fact]
        public void should_retrieve_added_booking()
        {
            // Arrange
            int hotelId = 1;
            int roomNumber = 1;
            int employeeId = 1;
            string roomType = "Deluxe";
            DateTime checkIn = DateTime.Today.AddDays(1);
            DateTime chekout = DateTime.Today.AddDays(2);

            // Act
            var newBooking = new Booking(roomNumber, hotelId, employeeId, roomType, checkIn, chekout);
            _bookingRepository.Add(newBooking);
            Booking? booking = _bookingRepository.Get(newBooking.BookingId);

            // Assert
            booking.Should().NotBeNull();
            booking.RoomNumber.Should().Be(1);
            booking.HotelId.Should().Be(1);
            booking.RoomType.Should().Be("Deluxe");
            booking.CheckIn.Should().BeSameDateAs(DateTime.Today.AddDays(1));
            booking.CheckOut.Should().BeSameDateAs(DateTime.Today.AddDays(2));
        }

    }
}
