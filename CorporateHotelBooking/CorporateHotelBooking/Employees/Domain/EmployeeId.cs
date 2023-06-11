using CorporateHotelBooking.Application.Employees.Domain;

namespace CorporateHotelBooking.Application.Employees.Domain;

public record EmployeeId(Guid Value)
{
    public static EmployeeId New()
    {
        return From(Guid.NewGuid());
    }
    public static EmployeeId From(Guid value)
    {
        return new EmployeeId(value);
    }
}
