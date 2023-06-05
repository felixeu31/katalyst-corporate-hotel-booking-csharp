using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Hotels.Domain;
using CorporateHotelBooking.Policies.Application;
using CorporateHotelBooking.Policies.Domain;
using FluentAssertions;
using Moq;

namespace CorporateHotelBooking.Test.Unit.UseCases
{
    public class IsBookingAllowedUseCaseTest
    {
        private readonly Mock<IPoliciesRepository> _policiesRepository;

        public IsBookingAllowedUseCaseTest()
        {
            _policiesRepository = new();
        }

        [Fact]
        public void should_return_true_when_policy_is_contained()
        {
            // Arrange
            Guid employeeId = Guid.NewGuid();
            var roomType = "Standard";
            var isBookingAllowedUseCase = new IsBookingAllowedUseCase(_policiesRepository.Object);
            _policiesRepository.Setup(x => x.GetEmployeePolicy(EmployeeId.From(employeeId))).Returns(new EmployeePolicy(EmployeeId.From(employeeId), new List<string>
                {
                    roomType
                }));

            // Act
            var isBookingAllowed = isBookingAllowedUseCase.Execute(employeeId, roomType);

            // Assert
            _policiesRepository.Verify(x => x.GetEmployeePolicy(EmployeeId.From(employeeId)), Times.Once());
            isBookingAllowed.Should().BeTrue();
        }
        [Fact]
        public void should_return_false_when_policy_is_not_contained()
        {
            // Arrange
            Guid employeeId = Guid.NewGuid();
            var roomType = "Standard";
            var anotherRoomType = "Deluxe";
            var isBookingAllowedUseCase = new IsBookingAllowedUseCase(_policiesRepository.Object);
            _policiesRepository.Setup(x => x.GetEmployeePolicy(EmployeeId.From(employeeId))).Returns(new EmployeePolicy(EmployeeId.From(employeeId), new List<string>
            {
                roomType
            }));

            // Act
            var isBookingAllowed = isBookingAllowedUseCase.Execute(employeeId, anotherRoomType);

            // Assert
            _policiesRepository.Verify(x => x.GetEmployeePolicy(EmployeeId.From(employeeId)), Times.Once());
            isBookingAllowed.Should().BeFalse();
        }
    }
}
