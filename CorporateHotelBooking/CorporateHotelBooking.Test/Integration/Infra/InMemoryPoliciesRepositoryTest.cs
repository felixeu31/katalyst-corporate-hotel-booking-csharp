﻿using CorporateHotelBooking.Data;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Policies.Domain;
using CorporateHotelBooking.Policies.Infrastructure;
using CorporateHotelBooking.Test.Constants;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Integration.Infra
{
    public class InMemoryPoliciesRepositoryTest
    {
        private readonly IPoliciesRepository _policiesRepository;
        private readonly InMemoryContext _context;

        public InMemoryPoliciesRepositoryTest()
        {
            _context = new InMemoryContext();
            _policiesRepository = new InMemoryPoliciesRepository(_context);
        }

        [Fact]
        public void should_add_employee_policy()
        {
            // Arrange
            var newEmployeePolicy = new EmployeePolicy(EmployeeId.New(), new List<string>{RoomTypes.Standard});

            // Act
            _policiesRepository.AddEmployeePolicy(newEmployeePolicy);

            // Assert
            EmployeePolicy? employeePolicy = _context.EmployeePolicies[newEmployeePolicy.EmployeeId];
            employeePolicy.Should().NotBeNull();
            employeePolicy.EmployeeId.Should().Be(newEmployeePolicy.EmployeeId);
            employeePolicy.RoomTypes.Should().BeEquivalentTo(newEmployeePolicy.RoomTypes);
        }

        [Fact]
        public void should_retrieve_employee_policy()
        {
            // Arrange
            var newEmployeePolicy = new EmployeePolicy(EmployeeId.New(), new List<string> { RoomTypes.Standard });
            _context.EmployeePolicies.Add(newEmployeePolicy.EmployeeId, newEmployeePolicy);

            // Act
            EmployeePolicy? employeePolicy = _policiesRepository.GetEmployeePolicy(newEmployeePolicy.EmployeeId);

            // Assert
            employeePolicy.Should().NotBeNull();
            employeePolicy.EmployeeId.Should().Be(newEmployeePolicy.EmployeeId);
            employeePolicy.RoomTypes.Should().BeEquivalentTo(newEmployeePolicy.RoomTypes);
        }


        [Fact]
        public void should_add_company_policy()
        {
            // Arrange
            var newCompanyPolicy = new CompanyPolicy(CompanyId.New(), new List<string> { RoomTypes.Standard });

            // Act
            _policiesRepository.AddCompanyPolicy(newCompanyPolicy);

            // Assert
            CompanyPolicy? companyPolicy = _context.CompanyPolicies[newCompanyPolicy.CompanyId];
            companyPolicy.Should().NotBeNull();
            companyPolicy.CompanyId.Should().Be(newCompanyPolicy.CompanyId);
            companyPolicy.RoomTypes.Should().BeEquivalentTo(newCompanyPolicy.RoomTypes);
        }

        [Fact]
        public void should_retrieve_company_policy()
        {
            // Arrange
            var newCompanyPolicy = new CompanyPolicy(CompanyId.New(), new List<string> { RoomTypes.Standard });
            _context.CompanyPolicies.Add(newCompanyPolicy.CompanyId, newCompanyPolicy);

            // Act
            CompanyPolicy? companyPolicy = _policiesRepository.GetCompanyPolicy(newCompanyPolicy.CompanyId);

            // Assert
            companyPolicy.Should().NotBeNull();
            companyPolicy.CompanyId.Should().Be(newCompanyPolicy.CompanyId);
            companyPolicy.RoomTypes.Should().BeEquivalentTo(newCompanyPolicy.RoomTypes);
        }

        [Fact]
        public void should_update_company_policy()
        {
            // Arrange
            var newCompanyPolicy = new CompanyPolicy(CompanyId.New(), new List<string> { RoomTypes.Standard });
            var updatedCompanyPolicy = new CompanyPolicy(newCompanyPolicy.CompanyId, new List<string> { RoomTypes.Deluxe });
            _context.CompanyPolicies.Add(newCompanyPolicy.CompanyId, newCompanyPolicy);

            // Act
            _policiesRepository.UpdateCompanyPolicy(updatedCompanyPolicy);

            // Assert
            CompanyPolicy? companyPolicy = _context.CompanyPolicies[updatedCompanyPolicy.CompanyId];
            companyPolicy.Should().NotBeNull();
            companyPolicy.CompanyId.Should().Be(updatedCompanyPolicy.CompanyId);
            companyPolicy.RoomTypes.Should().BeEquivalentTo(updatedCompanyPolicy.RoomTypes);
        }

        [Fact]
        public void should_update_employee_policy()
        {
            // Arrange
            var newEmployeePolicy = new EmployeePolicy(EmployeeId.New(), new List<string> { RoomTypes.Standard });
            var updatedEmployeePolicy = new EmployeePolicy(newEmployeePolicy.EmployeeId, new List<string> { RoomTypes.Deluxe });
            _context.EmployeePolicies.Add(newEmployeePolicy.EmployeeId, newEmployeePolicy);

            // Act
            _policiesRepository.UpdateEmployeePolicy(updatedEmployeePolicy);

            // Assert
            EmployeePolicy? employeePolicy = _context.EmployeePolicies[updatedEmployeePolicy.EmployeeId];
            employeePolicy.Should().NotBeNull();
            employeePolicy.EmployeeId.Should().Be(updatedEmployeePolicy.EmployeeId);
            employeePolicy.RoomTypes.Should().BeEquivalentTo(updatedEmployeePolicy.RoomTypes);
        }

    }
}