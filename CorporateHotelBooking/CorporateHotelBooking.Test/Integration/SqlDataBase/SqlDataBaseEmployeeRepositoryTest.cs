using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Data.Sql;
using CorporateHotelBooking.Data.Sql.DataModel;
using CorporateHotelBooking.Data.Sql.Repositories;
using CorporateHotelBooking.Test.Constants;
using CorporateHotelBooking.Test.Fixtures;
using FluentAssertions;

namespace CorporateHotelBooking.Test.Integration.SqlDataBase;

[Trait(TestTrait.Category, TestCategory.Integration)]
public class SqlDataBaseEmployeeRepositoryTest : IClassFixture<LocalDbTestFixture>
{
    private readonly LocalDbTestFixture _fixture;
    private readonly IEmployeeRepository _employeeRepository;

    public SqlDataBaseEmployeeRepositoryTest(LocalDbTestFixture fixture)
    {
        _fixture = fixture;
        _employeeRepository = new SqlEmployeeRepository(new CorporateHotelDbContext(fixture.DbContextOptions));
    }

    [Fact]
    public void should_retrieve_added_employee()
    {
        // Arrange
        var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        var employee1 = new Employee(CompanyId.New(), EmployeeId.New());

        // Act
        _employeeRepository.Add(employee1);

        // Assert
        EmployeeData? employee = context.Employees.Find(employee1.EmployeeId.Value);
        employee.Should().NotBeNull();
        employee.EmployeeId.Should().Be(employee1.EmployeeId.Value);
        employee.CompanyId.Should().Be(employee1.CompanyId.Value);
    }


    [Fact]
    public void should_delete_employee()
    {
        // Arrange
        var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        var employeeData = new EmployeeData
        {
            EmployeeId = Guid.NewGuid(),
            CompanyId = Guid.NewGuid()
        };
        context.Employees.Add(employeeData);
        context.SaveChanges();

        // Act
        _employeeRepository.Delete(EmployeeId.From(employeeData.EmployeeId));

        // Assert
        context.Employees.Should().NotContain(employeeData);
    }


    [Fact]
    public void should_get_employee()
    {
        // Arrange
        var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        var employeeData = new EmployeeData
        {
            EmployeeId = Guid.NewGuid(),
            CompanyId = Guid.NewGuid()
        };
        context.Employees.Add(employeeData);
        context.SaveChanges();

        // Act
        var employee = _employeeRepository.Get(EmployeeId.From(employeeData.EmployeeId));

        // Assert
        employee.Should().NotBeNull();
        employee.EmployeeId.Should().Be(EmployeeId.From(employeeData.EmployeeId));
        employee.CompanyId.Should().Be(CompanyId.From(employeeData.CompanyId));
    }

}