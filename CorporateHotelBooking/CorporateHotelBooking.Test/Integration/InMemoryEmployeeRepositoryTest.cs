﻿using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Employees.Infra;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Integration
{
    public class InMemoryEmployeeRepositoryTest
    {
        private readonly IEmployeeRepository _employeeRepository;

        public InMemoryEmployeeRepositoryTest()
        {
            _employeeRepository = new InMemoryEmployeeRepository();
        }

        [Fact]
        public void should_retrieve_added_employee()
        {
            // Arrange
            EmployeeId employeeId = EmployeeId.New();
            int companyId = 1;

            // Act
            _employeeRepository.Add(new Employee(companyId, employeeId));
            Employee? employee = _employeeRepository.Get(employeeId);

            // Assert
            employee.Should().NotBeNull();
            employee.CompanyId.Should().Be(1);
            employee.EmployeeId.Should().Be(employeeId);
        }

    }
}
