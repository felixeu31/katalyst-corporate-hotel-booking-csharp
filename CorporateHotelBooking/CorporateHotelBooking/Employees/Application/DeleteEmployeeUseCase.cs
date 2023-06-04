using CorporateHotelBooking.Employees.Domain;

namespace CorporateHotelBooking.Employees.Application;

public interface IDeleteEmployeeUseCase
{
    void Execute(Guid employeeId);
}

public class DeleteEmployeeUseCase : IDeleteEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;

    public DeleteEmployeeUseCase(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    public void Execute(Guid employeeId)
    {
        _employeeRepository.Delete(EmployeeId.From(employeeId));
    }
}