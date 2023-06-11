using System.ComponentModel.Design;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Application.Policies.UseCases;

namespace CorporateHotelBooking.Application.Policies.UseCases;

public class AddEmployeePolicyUseCase : IAddEmployeePolicyUseCase
{
    private readonly IPoliciesRepository _policiesRepository;

    public AddEmployeePolicyUseCase(IPoliciesRepository policiesRepository)
    {
        _policiesRepository = policiesRepository;
    }

    public void Execute(Guid employeeId, List<string> roomTypes)
    {
        if (_policiesRepository.GetEmployeePolicy(EmployeeId.From(employeeId)) != default)
        {
            _policiesRepository.UpdateEmployeePolicy(new EmployeePolicy(EmployeeId.From(employeeId), roomTypes));
        }
        _policiesRepository.AddEmployeePolicy(new EmployeePolicy(EmployeeId.From(employeeId), roomTypes));
    }
}
