namespace CorporateHotelBooking.Application.Employees.UseCases;

public interface IAddEmployeeUseCase
{
    void Execute(Guid companyId, Guid employeeId);
}
