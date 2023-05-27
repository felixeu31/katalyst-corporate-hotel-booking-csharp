using CorporateHotelBooking.Hotels.Domain;
using ValueOf;

namespace CorporateHotelBooking.Employees.Domain;

public class EmployeeId : ValueOf<Guid, EmployeeId>
{
    public static EmployeeId New()
    {
        return From(Guid.NewGuid());
    }
}