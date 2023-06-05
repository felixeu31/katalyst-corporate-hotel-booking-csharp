using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Employees.Application;

public class AddEmployeeUseCase : IAddEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;

    public AddEmployeeUseCase(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public void Execute(Guid companyId, Guid employeeId)
    {
        _employeeRepository.Add(new Employee(CompanyId.From(companyId), EmployeeId.From(employeeId)));
    }
}