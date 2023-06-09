using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Policies.Domain;
using System.ComponentModel.Design;

namespace CorporateHotelBooking.Policies.Application;

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