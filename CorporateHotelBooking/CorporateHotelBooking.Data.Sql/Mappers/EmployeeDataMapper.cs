﻿using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Data.Sql.DataModel;

namespace CorporateHotelBooking.Data.Sql.Mappers;

public class EmployeeDataMapper
{
    public static EmployeeData MapEmployeeDataFrom(Employee employee)
    {
        return new EmployeeData
        {
            EmployeeId = employee.EmployeeId.Value,
            CompanyId = employee.CompanyId.Value
        };
    }

    public static Employee MapEmployeeFrom(EmployeeData employeeData)
    {
        return new Employee(CompanyId.From(employeeData.CompanyId), EmployeeId.From(employeeData.EmployeeId));
    }
}