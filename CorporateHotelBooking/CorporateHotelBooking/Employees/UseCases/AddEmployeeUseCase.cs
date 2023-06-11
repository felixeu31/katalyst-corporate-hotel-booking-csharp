using CorporateHotelBooking.Application.Employees.Domain;

namespace CorporateHotelBooking.Application.Employees.UseCases;

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
