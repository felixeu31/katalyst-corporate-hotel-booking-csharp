using ValueOf;

namespace CorporateHotelBooking.Employees.Domain;

public class CompanyId : ValueOf<Guid, CompanyId>
{
    public static CompanyId New()
    {
        return From(Guid.NewGuid());
    }
}