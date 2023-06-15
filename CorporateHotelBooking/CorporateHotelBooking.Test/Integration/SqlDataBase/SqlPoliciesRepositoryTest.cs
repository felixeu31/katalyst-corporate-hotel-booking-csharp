using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Data.Sql.Repositories;
using CorporateHotelBooking.Data.Sql;
using CorporateHotelBooking.Test.Fixtures.DataBase;
using CorporateHotelBooking.Test.Constants;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Data.Sql.DataModel;
using CorporateHotelBooking.Test.TestUtilities;
using FluentAssertions;
using CorporateHotelBooking.Application.Policies.Domain;
using Microsoft.EntityFrameworkCore;

namespace CorporateHotelBooking.Test.Integration.SqlDataBase;

[Collection(nameof(LocalDbTestFixtureCollection))]
[Trait(TestTrait.Category, TestCategory.Integration)]
public class SqlPoliciesRepositoryTest
{
    private readonly LocalDbTestFixture _fixture;
    private readonly IPoliciesRepository _policiesRepository;
    public SqlPoliciesRepositoryTest(LocalDbTestFixture fixture)
    {
        _fixture = fixture;
        _policiesRepository = new SqlPoliciesRepository(new CorporateHotelDbContext(fixture.DbContextOptions));
    }


    [Fact]
    public void should_add_employee_policy()
    {
        // Arrange
        var newEmployeePolicy = new EmployeePolicy(EmployeeId.New(), new List<string> { SampleData.RoomTypes.Standard });

        // Act
        _policiesRepository.AddEmployeePolicy(newEmployeePolicy);

        // Assert
        using var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        EmployeePolicyData? employeePolicy = context.EmployeePolicies.Find( newEmployeePolicy.EmployeeId.Value);
        employeePolicy.Should().NotBeNull();
        employeePolicy.EmployeeId.Should().Be(newEmployeePolicy.EmployeeId.Value);
        employeePolicy.RoomTypes.Split(";").Should().BeEquivalentTo(newEmployeePolicy.RoomTypes);
    }

    //[Fact]
    //public void should_retrieve_employee_policy()
    //{
    //    // Arrange
    //    var newEmployeePolicy = new EmployeePolicy(EmployeeId.New(), new List<string> { SampleData.RoomTypes.Standard });
    //    _context.EmployeePolicies.Add(newEmployeePolicy.EmployeeId, newEmployeePolicy);

    //    // Act
    //    EmployeePolicy? employeePolicy = _policiesRepository.GetEmployeePolicy(newEmployeePolicy.EmployeeId);

    //    // Assert
    //    employeePolicy.Should().NotBeNull();
    //    employeePolicy.EmployeeId.Should().Be(newEmployeePolicy.EmployeeId);
    //    employeePolicy.RoomTypes.Should().BeEquivalentTo(newEmployeePolicy.RoomTypes);
    //}


    //[Fact]
    //public void should_add_company_policy()
    //{
    //    // Arrange
    //    var newCompanyPolicy = new CompanyPolicy(CompanyId.New(), new List<string> { SampleData.RoomTypes.Standard });

    //    // Act
    //    _policiesRepository.AddCompanyPolicy(newCompanyPolicy);

    //    // Assert
    //    CompanyPolicy? companyPolicy = _context.CompanyPolicies[newCompanyPolicy.CompanyId];
    //    companyPolicy.Should().NotBeNull();
    //    companyPolicy.CompanyId.Should().Be(newCompanyPolicy.CompanyId);
    //    companyPolicy.RoomTypes.Should().BeEquivalentTo(newCompanyPolicy.RoomTypes);
    //}

    //[Fact]
    //public void should_retrieve_company_policy()
    //{
    //    // Arrange
    //    var newCompanyPolicy = new CompanyPolicy(CompanyId.New(), new List<string> { SampleData.RoomTypes.Standard });
    //    _context.CompanyPolicies.Add(newCompanyPolicy.CompanyId, newCompanyPolicy);

    //    // Act
    //    CompanyPolicy? companyPolicy = _policiesRepository.GetCompanyPolicy(newCompanyPolicy.CompanyId);

    //    // Assert
    //    companyPolicy.Should().NotBeNull();
    //    companyPolicy.CompanyId.Should().Be(newCompanyPolicy.CompanyId);
    //    companyPolicy.RoomTypes.Should().BeEquivalentTo(newCompanyPolicy.RoomTypes);
    //}

    //[Fact]
    //public void should_update_company_policy()
    //{
    //    // Arrange
    //    var newCompanyPolicy = new CompanyPolicy(CompanyId.New(), new List<string> { SampleData.RoomTypes.Standard });
    //    var updatedCompanyPolicy = new CompanyPolicy(newCompanyPolicy.CompanyId, new List<string> { SampleData.RoomTypes.Deluxe });
    //    _context.CompanyPolicies.Add(newCompanyPolicy.CompanyId, newCompanyPolicy);

    //    // Act
    //    _policiesRepository.UpdateCompanyPolicy(updatedCompanyPolicy);

    //    // Assert
    //    CompanyPolicy? companyPolicy = _context.CompanyPolicies[updatedCompanyPolicy.CompanyId];
    //    companyPolicy.Should().NotBeNull();
    //    companyPolicy.CompanyId.Should().Be(updatedCompanyPolicy.CompanyId);
    //    companyPolicy.RoomTypes.Should().BeEquivalentTo(updatedCompanyPolicy.RoomTypes);
    //}

    //[Fact]
    //public void should_update_employee_policy()
    //{
    //    // Arrange
    //    var newEmployeePolicy = new EmployeePolicy(EmployeeId.New(), new List<string> { SampleData.RoomTypes.Standard });
    //    var updatedEmployeePolicy = new EmployeePolicy(newEmployeePolicy.EmployeeId, new List<string> { SampleData.RoomTypes.Deluxe });
    //    _context.EmployeePolicies.Add(newEmployeePolicy.EmployeeId, newEmployeePolicy);

    //    // Act
    //    _policiesRepository.UpdateEmployeePolicy(updatedEmployeePolicy);

    //    // Assert
    //    EmployeePolicy? employeePolicy = _context.EmployeePolicies[updatedEmployeePolicy.EmployeeId];
    //    employeePolicy.Should().NotBeNull();
    //    employeePolicy.EmployeeId.Should().Be(updatedEmployeePolicy.EmployeeId);
    //    employeePolicy.RoomTypes.Should().BeEquivalentTo(updatedEmployeePolicy.RoomTypes);
    //}

    //[Fact]
    //public void should_delete_employee_policies()
    //{
    //    // Arrange
    //    var newEmployeePolicy = new EmployeePolicy(EmployeeId.New(), new List<string> { SampleData.RoomTypes.Standard });
    //    _context.EmployeePolicies.Add(newEmployeePolicy.EmployeeId, newEmployeePolicy);

    //    // Act
    //    _policiesRepository.DeleteEmployeePolicies(newEmployeePolicy.EmployeeId);

    //    // Assert
    //    var containsEmployeePolicy = _context.EmployeePolicies.ContainsKey(newEmployeePolicy.EmployeeId);
    //    containsEmployeePolicy.Should().BeFalse();
    //}
}