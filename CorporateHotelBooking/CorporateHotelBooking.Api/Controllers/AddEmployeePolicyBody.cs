namespace CorporateHotelBooking.Api.Controllers;

public record AddEmployeePolicyBody(Guid EmployeeId, List<string> Policies);