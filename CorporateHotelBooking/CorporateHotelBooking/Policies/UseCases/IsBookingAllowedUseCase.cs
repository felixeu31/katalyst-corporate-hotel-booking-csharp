using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Employees.Domain.Exceptions;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Application.Policies.UseCases;

namespace CorporateHotelBooking.Application.Policies.UseCases;

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
        var employee = _employeeRepository.Get(EmployeeId.From(employeeId)) ?? throw new EmployeeNotFoundException();

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
