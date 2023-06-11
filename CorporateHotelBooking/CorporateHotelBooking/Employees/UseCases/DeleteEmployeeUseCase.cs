using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Policies.Domain;

namespace CorporateHotelBooking.Application.Employees.UseCases;

public interface IDeleteEmployeeUseCase
{
    void Execute(Guid employeeId);
}

public class DeleteEmployeeUseCase : IDeleteEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPoliciesRepository _policiesRepository;
    private readonly IBookingRepository _bookingRepository;

    public DeleteEmployeeUseCase(IEmployeeRepository employeeRepository, IPoliciesRepository policiesRepository, IBookingRepository bookingRepository)
    {
        _employeeRepository = employeeRepository;
        _policiesRepository = policiesRepository;
        _bookingRepository = bookingRepository;
    }
    public void Execute(Guid employeeId)
    {
        _policiesRepository.DeleteEmployeePolicies(EmployeeId.From(employeeId));
        _bookingRepository.DeleteEmployeeBookings(EmployeeId.From(employeeId));
        _employeeRepository.Delete(EmployeeId.From(employeeId));
    }
}
