using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Policies.Domain;

public class CompanyPolicy
{
    public CompanyId CompanyId { get; }
    public List<string> RoomTypes { get; }

    public CompanyPolicy(CompanyId employeeId, List<string> roomTypes)
    {
        CompanyId = employeeId;
        RoomTypes = roomTypes;
    }
}