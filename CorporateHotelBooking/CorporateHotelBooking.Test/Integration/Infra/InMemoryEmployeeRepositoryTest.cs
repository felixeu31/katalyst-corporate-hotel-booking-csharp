using CorporateHotelBooking.Data;
using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Employees.Infra;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Integration.Infra
{
    public class InMemoryEmployeeRepositoryTest
    {
        private readonly IEmployeeRepository _employeeRepository;
        private InMemoryContext _context;

        public InMemoryEmployeeRepositoryTest()
        {
            _context = new InMemoryContext();
            _employeeRepository = new InMemoryEmployeeRepository(_context);
        }

        [Fact]
        public void should_retrieve_added_employee()
        {
            // Arrange
            EmployeeId employeeId = EmployeeId.New();
            CompanyId companyId = CompanyId.New();

            // Act
            _employeeRepository.Add(new Employee(companyId, employeeId));
            Employee? employee = _employeeRepository.Get(employeeId);

            // Assert
            employee.Should().NotBeNull();
            employee.CompanyId.Should().Be(companyId);
            employee.EmployeeId.Should().Be(employeeId);
        }


        [Fact]
        public void should_delete_employee()
        {
            // Arrange
            EmployeeId employeeId = EmployeeId.New();
            CompanyId companyId = CompanyId.New();
            _context.Employees.Add(employeeId, new Employee(companyId, employeeId));

            // Act
            _employeeRepository.Delete(employeeId);

            // Assert
            _context.Employees.Should().NotContainKey(employeeId);
        }

    }
}
