using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.UseCases;
using CorporateHotelBooking.Data.Sql.DataModel;

namespace CorporateHotelBooking.Data.Sql.Mappers;

public class EmployeeDataMapper
{
    public static EmployeeData? MapEmployeeDataFrom(Employee? employee)
    {
        if (employee is null) return null;

        return new EmployeeData
        {
            EmployeeId = employee.EmployeeId.Value,
            CompanyId = employee.CompanyId.Value
        };
    }

    public static Employee? MapEmployeeFrom(EmployeeData? employeeData)
    {
        if (employeeData is null) return null;

        return new Employee(CompanyId.From(employeeData.CompanyId), EmployeeId.From(employeeData.EmployeeId));
    }
}