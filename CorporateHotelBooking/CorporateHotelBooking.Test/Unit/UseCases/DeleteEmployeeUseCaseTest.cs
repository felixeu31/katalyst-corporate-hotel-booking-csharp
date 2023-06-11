﻿using CorporateHotelBooking.Employees.Application;
using Moq;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Policies.Domain;

namespace CorporateHotelBooking.Test.Unit.UseCases
{
    public class DeleteEmployeeUseCaseTest
    {
        private readonly Mock<IEmployeeRepository> _employeeRepository;
        private readonly Mock<IPoliciesRepository> _policiesRepository;

        public DeleteEmployeeUseCaseTest()
        {
            _employeeRepository = new();
            _policiesRepository = new();
        }

        [Fact]
        public void DeleteEmployee_ShouldRemoveEmployeeAndRelatedEntities()
        {
            // Arrange
            var addEmployeeUseCase = new DeleteEmployeeUseCase(_employeeRepository.Object, _policiesRepository.Object);
            var employeeId = Guid.NewGuid();

            // Act
            addEmployeeUseCase.Execute(employeeId);

            // Assert
            _employeeRepository.Verify(x => x.Delete(It.IsAny<EmployeeId>()), Times.Once());
            _policiesRepository.Verify(x => x.DeleteEmployeePolicies(It.IsAny<EmployeeId>()), Times.Once());

        }
    }
}
