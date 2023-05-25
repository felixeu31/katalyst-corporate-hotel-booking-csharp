using CorporateHotelBooking.Employees.Application;
using CorporateHotelBooking.Hotels.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Test.Unit
{
    public class EmployeeServiceUnitTest
    {
        private readonly Mock<IEmployeeRepository> _employeeRepository;

        public EmployeeServiceUnitTest()
        {
            _employeeRepository = new();
        }

        [Fact]
        public void AddEmployee_ShouldStoreEmployee()
        {
            // Arrange
            var employeeService = new EmployeeService(_employeeRepository.Object);
            int employeeId = 1;
            int companyId = 1;

            // Act
            employeeService.AddEmployee(companyId, employeeId);

            // Assert
            _employeeRepository.Verify(x => x.Add(It.IsAny<Employee>()), Times.Once());

        }
    }
}
