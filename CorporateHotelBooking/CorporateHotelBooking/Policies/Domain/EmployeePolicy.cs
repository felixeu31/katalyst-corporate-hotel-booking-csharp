using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Policies.Domain;

public class EmployeePolicy
{
    public EmployeeId EmployeeId { get; }
    public List<string> RoomTypes { get; }

    public EmployeePolicy(EmployeeId employeeId, List<string> roomTypes)
    {
        EmployeeId = employeeId;
        RoomTypes = roomTypes;
    }
}