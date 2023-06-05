using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Policies.Domain;

namespace CorporateHotelBooking.Policies.Application;

public class IsBookingAllowedUseCase : IIsBookingAllowedUseCase
{
    private readonly IPoliciesRepository _policiesRepository;

    public IsBookingAllowedUseCase(IPoliciesRepository policiesRepository)
    {
        _policiesRepository = policiesRepository;
    }

    public bool Execute(Guid employeeId, string roomType)
    {
        var employeePolicy = _policiesRepository.GetEmployeePolicy(EmployeeId.From(employeeId));

        if (employeePolicy != null && employeePolicy.RoomTypes.Contains(roomType))
        {
            return true;
        }

        return false;

    }
}