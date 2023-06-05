namespace CorporateHotelBooking.Employees.Application;

public interface IAddEmployeeUseCase
{
    void Execute(Guid companyId, Guid employeeId);
}