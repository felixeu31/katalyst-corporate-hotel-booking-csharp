using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Hotels.UseCases;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Data.Sql.DataModel;

namespace CorporateHotelBooking.Data.Sql.Mappers;

public class EmployeePolicyDataMapper
{
    public static EmployeePolicyData? MapEmployeePolicyDataFrom(EmployeePolicy? employeePolicy)
    {
        if (employeePolicy is null) return null;

        return new EmployeePolicyData
        {
            EmployeeId = employeePolicy.EmployeeId.Value,
            RoomTypes = string.Join(";", employeePolicy.RoomTypes)
        };
    }

    public static EmployeePolicy? HydrateDomainFrom(EmployeePolicyData? employeePolicyData)
    {
        if (employeePolicyData is null) return null;

        return new EmployeePolicy(EmployeeId.From(employeePolicyData.EmployeeId), employeePolicyData.RoomTypes.Split(";").ToList());
    }

    public static void ApplyDomainChanges(EmployeePolicyData employeePolicyData, EmployeePolicy employeePolicy)
    {
        employeePolicyData.RoomTypes = string.Join(";", employeePolicy.RoomTypes);
    }
}