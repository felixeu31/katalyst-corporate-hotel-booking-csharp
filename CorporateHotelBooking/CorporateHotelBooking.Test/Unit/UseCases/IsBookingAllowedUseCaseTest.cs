using FluentAssertions;
using Moq;
using System.ComponentModel.Design;
using CorporateHotelBooking.Test.Constants;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Employees.Domain.Exceptions;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Application.Policies.UseCases;

namespace CorporateHotelBooking.Test.Unit.UseCases
{
    public class IsBookingAllowedUseCaseTest
    {
        private readonly Mock<IPoliciesRepository> _policiesRepository;
        private readonly Mock<IEmployeeRepository> _employeeRepository;

        public IsBookingAllowedUseCaseTest()
        {
            _policiesRepository = new();
            _employeeRepository = new();
        }

        [Fact]
        public void should_return_true_when_policy_is_contained_in_employee_policy()
        {
            // Arrange
            Guid companyId = Guid.NewGuid();
            Guid employeeId = Guid.NewGuid();
            var roomType = SampleData.RoomTypes.Standard;
            var isBookingAllowedUseCase = new IsBookingAllowedUseCase(_policiesRepository.Object, _employeeRepository.Object);
            _employeeRepository.Setup(x => x.Get(EmployeeId.From(employeeId)))
                .Returns(new Employee(CompanyId.From(companyId), EmployeeId.From(employeeId)));
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
        public void should_return_false_when_policy_is_not_contained_in_employee_policy()
        {
            // Arrange
            Guid companyId = Guid.NewGuid();
            Guid employeeId = Guid.NewGuid();
            var roomType = SampleData.RoomTypes.Standard;
            var anotherRoomType = SampleData.RoomTypes.Deluxe;
            var isBookingAllowedUseCase = new IsBookingAllowedUseCase(_policiesRepository.Object, _employeeRepository.Object);
            _employeeRepository.Setup(x => x.Get(EmployeeId.From(employeeId)))
                .Returns(new Employee(CompanyId.From(companyId), EmployeeId.From(employeeId)));
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

        [Fact]
        public void should_return_true_when_employee_policy_does_not_exist()
        {
            // Arrange
            Guid companyId = Guid.NewGuid();
            Guid employeeId = Guid.NewGuid();
            var roomType = SampleData.RoomTypes.Standard;
            var anotherRoomType = SampleData.RoomTypes.Deluxe;
            var isBookingAllowedUseCase = new IsBookingAllowedUseCase(_policiesRepository.Object, _employeeRepository.Object);
            _employeeRepository.Setup(x => x.Get(EmployeeId.From(employeeId)))
                .Returns(new Employee(CompanyId.From(companyId), EmployeeId.From(employeeId)));
            _policiesRepository.Setup(x => x.GetEmployeePolicy(EmployeeId.From(employeeId))).Returns(default(EmployeePolicy?));

            // Act
            var isBookingAllowed = isBookingAllowedUseCase.Execute(employeeId, anotherRoomType);

            // Assert
            _policiesRepository.Verify(x => x.GetEmployeePolicy(EmployeeId.From(employeeId)), Times.Once());
            isBookingAllowed.Should().BeTrue();
        }

        [Fact]
        public void should_return_true_when_policy_is_contained_in_company_policy()
        {
            // Arrange
            Guid companyId = Guid.NewGuid();
            Guid employeeId = Guid.NewGuid();
            var roomType = SampleData.RoomTypes.Standard;
            var isBookingAllowedUseCase = new IsBookingAllowedUseCase(_policiesRepository.Object, _employeeRepository.Object);
            _employeeRepository.Setup(x => x.Get(EmployeeId.From(employeeId)))
                .Returns(new Employee(CompanyId.From(companyId), EmployeeId.From(employeeId)));
            _policiesRepository.Setup(x => x.GetEmployeePolicy(EmployeeId.From(employeeId))).Returns(default(EmployeePolicy?));
            _policiesRepository.Setup(x => x.GetCompanyPolicy(CompanyId.From(companyId))).Returns(new CompanyPolicy(CompanyId.From(companyId), new List<string>
            {
                roomType
            }));

            // Act
            var isBookingAllowed = isBookingAllowedUseCase.Execute(employeeId, roomType);

            // Assert
            _policiesRepository.Verify(x => x.GetEmployeePolicy(EmployeeId.From(employeeId)), Times.Once());
            _policiesRepository.Verify(x => x.GetCompanyPolicy(CompanyId.From(companyId)), Times.Once());
            isBookingAllowed.Should().BeTrue();
        }

        [Fact]
        public void should_return_false_when_policy_is_not_contained_in_company_policy()
        {
            // Arrange
            Guid companyId = Guid.NewGuid();
            Guid employeeId = Guid.NewGuid();
            var roomType = SampleData.RoomTypes.Standard;
            var anotherRoomType = SampleData.RoomTypes.Deluxe;
            var isBookingAllowedUseCase = new IsBookingAllowedUseCase(_policiesRepository.Object, _employeeRepository.Object);
            _employeeRepository.Setup(x => x.Get(EmployeeId.From(employeeId)))
                .Returns(new Employee(CompanyId.From(companyId), EmployeeId.From(employeeId)));
            _policiesRepository.Setup(x => x.GetEmployeePolicy(EmployeeId.From(employeeId))).Returns(default(EmployeePolicy?));
            _policiesRepository.Setup(x => x.GetCompanyPolicy(CompanyId.From(companyId))).Returns(new CompanyPolicy(CompanyId.From(companyId), new List<string>
            {
                roomType
            }));

            // Act
            var isBookingAllowed = isBookingAllowedUseCase.Execute(employeeId, anotherRoomType);

            // Assert
            _policiesRepository.Verify(x => x.GetEmployeePolicy(EmployeeId.From(employeeId)), Times.Once());
            _policiesRepository.Verify(x => x.GetCompanyPolicy(CompanyId.From(companyId)), Times.Once());
            isBookingAllowed.Should().BeFalse();
        }

        [Fact]
        public void should_return_true_when_policy_if_company_policy_not_exists()
        {
            // Arrange
            Guid companyId = Guid.NewGuid();
            Guid employeeId = Guid.NewGuid();
            var roomType = SampleData.RoomTypes.Standard;
            var isBookingAllowedUseCase = new IsBookingAllowedUseCase(_policiesRepository.Object, _employeeRepository.Object);
            _employeeRepository.Setup(x => x.Get(EmployeeId.From(employeeId)))
                .Returns(new Employee(CompanyId.From(companyId), EmployeeId.From(employeeId)));
            _policiesRepository.Setup(x => x.GetEmployeePolicy(EmployeeId.From(employeeId))).Returns(default(EmployeePolicy?));
            _policiesRepository.Setup(x => x.GetCompanyPolicy(CompanyId.From(companyId))).Returns(default(CompanyPolicy?));

            // Act
            var isBookingAllowed = isBookingAllowedUseCase.Execute(employeeId, roomType);

            // Assert
            _policiesRepository.Verify(x => x.GetEmployeePolicy(EmployeeId.From(employeeId)), Times.Once());
            _policiesRepository.Verify(x => x.GetCompanyPolicy(CompanyId.From(companyId)), Times.Once());
            isBookingAllowed.Should().BeTrue();
        }
        
        [Fact]
        public void should_throw_exception_when_employee_does_not_exist()
        {
            // Arrange
            Guid companyId = Guid.NewGuid();
            Guid employeeId = Guid.NewGuid();
            var roomType = SampleData.RoomTypes.Standard;
            var isBookingAllowedUseCase = new IsBookingAllowedUseCase(_policiesRepository.Object, _employeeRepository.Object);
            _employeeRepository.Setup(x => x.Get(EmployeeId.From(employeeId)))
                .Returns(default(Employee?));

            // Act
            Action action = () => isBookingAllowedUseCase.Execute(employeeId, roomType);

            // Assert
            action.Should().Throw<EmployeeNotFoundException>();
        }
    }
}
