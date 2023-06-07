using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Policies.Domain;

namespace CorporateHotelBooking.Policies.Application;

public class IsBookingAllowedUseCase : IIsBookingAllowedUseCase
{
    private readonly IPoliciesRepository _policiesRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public IsBookingAllowedUseCase(IPoliciesRepository policiesRepository, IEmployeeRepository employeeRepository)
    {
        _policiesRepository = policiesRepository;
        _employeeRepository = employeeRepository;
    }

    public bool Execute(Guid employeeId, string roomType)
    {
        var employee = _employeeRepository.Get(EmployeeId.From(employeeId));

        var employeePolicy = _policiesRepository.GetEmployeePolicy(EmployeeId.From(employeeId));
        var companyPolicy = _policiesRepository.GetCompanyPolicy(employee.CompanyId);

        if (employeePolicy != null)
        {
            return employeePolicy.RoomTypes.Contains(roomType);
        }

        if (companyPolicy != null)
        {
            return companyPolicy.RoomTypes.Contains(roomType);
        }

        return true;

    }
}