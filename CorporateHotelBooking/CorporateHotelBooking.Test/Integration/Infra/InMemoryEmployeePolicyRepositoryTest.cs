using CorporateHotelBooking.Data;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Policies.Domain;
using CorporateHotelBooking.Policies.Infrastructure;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Integration.Infra
{
    public class InMemoryPoliciesRepositoryTest
    {
        private readonly IPoliciesRepository _employeePolicyRepository;
        private readonly InMemoryContext _context;

        public InMemoryPoliciesRepositoryTest()
        {
            _context = new InMemoryContext();
            _employeePolicyRepository = new InMemoryPoliciesRepository(_context);
        }

        [Fact]
        public void should_retrieve_added_employee_policy()
        {
            // Arrange
            var newEmployeePolicy = new EmployeePolicy(EmployeeId.New(), new List<string>{"Standard"});

            // Act
            _employeePolicyRepository.AddEmployeePolicy(newEmployeePolicy);

            // Assert
            EmployeePolicy? employeePolicy = _context.EmployeePolicies[newEmployeePolicy.EmployeeId];
            employeePolicy.Should().NotBeNull();
            employeePolicy.EmployeeId.Should().Be(newEmployeePolicy.EmployeeId);
            employeePolicy.Policies.Should().BeEquivalentTo(newEmployeePolicy.Policies);
        }

    }
}
