using FluentAssertions;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Data.InMemory;
using CorporateHotelBooking.Data.InMemory.Repositories;

namespace CorporateHotelBooking.Test.Integration.Infra
{
    public class InMemoryEmployeeRepositoryTest
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly InMemoryContext _context;

        public InMemoryEmployeeRepositoryTest()
        {
            _context = new InMemoryContext();
            _employeeRepository = new InMemoryEmployeeRepository(_context);
        }

        [Fact]
        public void should_retrieve_added_employee()
        {
            // Arrange
            var newEmployee = new Employee(CompanyId.New(), EmployeeId.New());

            // Act
            _employeeRepository.Add(newEmployee);

            // Assert
            Employee? employee = _context.Employees[newEmployee.EmployeeId];
            employee.Should().NotBeNull();
            employee.CompanyId.Should().Be(newEmployee.CompanyId);
            employee.EmployeeId.Should().Be(newEmployee.EmployeeId);
        }


        [Fact]
        public void should_delete_employee()
        {
            // Arrange
            var employee = new Employee(CompanyId.New(), EmployeeId.New());
            _context.Employees.Add(employee.EmployeeId, employee);

            // Act
            _employeeRepository.Delete(employee.EmployeeId);

            // Assert
            _context.Employees.Should().NotContainKey(EmployeeId.New());
        }

    }
}
