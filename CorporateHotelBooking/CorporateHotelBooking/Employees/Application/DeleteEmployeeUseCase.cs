using CorporateHotelBooking.Employees.Domain;
using CorporateHotelBooking.Policies.Domain;

namespace CorporateHotelBooking.Employees.Application;

public interface IDeleteEmployeeUseCase
{
    void Execute(Guid employeeId);
}

public class DeleteEmployeeUseCase : IDeleteEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPoliciesRepository _policiesRepository;

    public DeleteEmployeeUseCase(IEmployeeRepository employeeRepository, IPoliciesRepository policiesRepository)
    {
        _employeeRepository = employeeRepository;
        _policiesRepository = policiesRepository;
    }
    public void Execute(Guid employeeId)
    {
        _policiesRepository.DeleteEmployeePolicies(EmployeeId.From(employeeId));
        _employeeRepository.Delete(EmployeeId.From(employeeId));
    }
}