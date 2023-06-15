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

    [Fact]
    public void should_retrieve_employee_policy()
    {
        // Arrange
        using var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        var newEmployeePolicyData = new EmployeePolicyData
        {
            EmployeeId = EmployeeId.New().Value,
            RoomTypes = "junior;deluxe"
        };
        context.EmployeePolicies.Add(newEmployeePolicyData);
        context.SaveChanges();

        // Act
        EmployeePolicy? employeePolicy = _policiesRepository.GetEmployeePolicy(EmployeeId.From(newEmployeePolicyData.EmployeeId));

        // Assert
        employeePolicy.Should().NotBeNull();
        employeePolicy.EmployeeId.Should().Be(EmployeeId.From(newEmployeePolicyData.EmployeeId));
        employeePolicy.RoomTypes.Should().BeEquivalentTo(newEmployeePolicyData.RoomTypes.Split(";"));
    }


    [Fact]
    public void should_add_company_policy()
    {
        // Arrange
        var newCompanyPolicy = new CompanyPolicy(CompanyId.New(), new List<string> { SampleData.RoomTypes.Standard });

        // Act
        _policiesRepository.AddCompanyPolicy(newCompanyPolicy);

        // Assert
        using var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        CompanyPolicyData? companyPolicy = context.CompanyPolicies.Find(newCompanyPolicy.CompanyId.Value);
        companyPolicy.Should().NotBeNull();
        companyPolicy.CompanyId.Should().Be(newCompanyPolicy.CompanyId.Value);
        companyPolicy.RoomTypes.Split(";").Should().BeEquivalentTo(newCompanyPolicy.RoomTypes);
    }

    [Fact]
    public void should_retrieve_company_policy()
    {
        // Arrange
        using var context = new CorporateHotelDbContext(_fixture.DbContextOptions);
        var newCompanyPolicyData = new CompanyPolicyData
        {
            CompanyId = CompanyId.New().Value,
            RoomTypes = "junior;deluxe"
        };
        context.CompanyPolicies.Add(newCompanyPolicyData);
        context.SaveChanges();

        // Act
        CompanyPolicy? companyPolicy = _policiesRepository.GetCompanyPolicy(CompanyId.From(newCompanyPolicyData.CompanyId));

        // Assert
        companyPolicy.Should().NotBeNull();
        companyPolicy.CompanyId.Should().Be(CompanyId.From(newCompanyPolicyData.CompanyId));
        companyPolicy.RoomTypes.Should().BeEquivalentTo(newCompanyPolicyData.RoomTypes.Split(";"));
    }

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