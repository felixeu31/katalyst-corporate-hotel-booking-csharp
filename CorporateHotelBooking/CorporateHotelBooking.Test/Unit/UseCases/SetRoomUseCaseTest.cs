using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorporateHotelBooking.Hotels.Application;
using CorporateHotelBooking.Hotels.Domain;
using CorporateHotelBooking.Hotels.Domain.Exceptions;
using CorporateHotelBooking.Test.Constants;
using FluentAssertions;
using Moq;

namespace CorporateHotelBooking.Test.Unit.UseCases
{
    public class SetRoomUseCaseTest
    {
        private readonly Mock<IHotelRepository> _hotelRepository;

        public SetRoomUseCaseTest()
        {
            _hotelRepository = new();
        }

        [Fact]
        public void should_add_room_when_does_not_exist_in_hotel()
        {
            // Arrange
            Guid hotelId = Guid.NewGuid();
            string hotelName = "Westing";
            var roomNumber = 1;
            var roomType = RoomTypes.Deluxe;
            var hotel = new Hotel(HotelId.From(hotelId), hotelName);
            _hotelRepository.Setup(repository => repository.Get(HotelId.From(hotelId))).Returns(hotel);
            var setRoomUseCase = new SetRoomUseCase(_hotelRepository.Object);

            // Act
            setRoomUseCase.Execute(hotelId, roomNumber, roomType);

            // Assert
            _hotelRepository.Verify(x => x.Update(It.IsAny<Hotel>()), Times.Once());
        }


        [Fact]
        public void should_update_room_when_already_exists_in_hotel()
        {
            // Arrange
            Guid hotelId = Guid.NewGuid();
            string hotelName = "Westing";
            var roomNumber = 1;
            var roomType = RoomTypes.Deluxe;
            var otherRoomType = RoomTypes.Standard;
            var hotel = new Hotel(HotelId.From(hotelId), hotelName);
            hotel.SetRoom(roomNumber, roomType);
            _hotelRepository.Setup(repository => repository.Get(HotelId.From(hotelId))).Returns(hotel);
            var setRoomUseCase = new SetRoomUseCase(_hotelRepository.Object);

            // Act
            setRoomUseCase.Execute(hotelId, roomNumber, roomType);
            setRoomUseCase.Execute(hotelId, roomNumber, otherRoomType);

            // Assert
            _hotelRepository.Verify(x => x.Update(It.IsAny<Hotel>()));
        }

        [Fact]
        public void should_throw_exception_when_hotel_not_found()
        {
            // Arrange
            Guid hotelId = Guid.NewGuid();
            string hotelName = "Westing";
            var roomNumber = 1;
            var roomType = RoomTypes.Deluxe;
            var hotel = new Hotel(HotelId.From(hotelId), hotelName);
            hotel.SetRoom(roomNumber, roomType);
            _hotelRepository.Setup(repository => repository.Get(HotelId.From(hotelId))).Returns(default(Hotel?));
            var setRoomUseCase = new SetRoomUseCase(_hotelRepository.Object);

            // Act
            Action action = () => setRoomUseCase.Execute(hotelId, roomNumber, roomType);

            // Assert
            action.Should().Throw<HotelNotFoundException>();
        }
    }
}
