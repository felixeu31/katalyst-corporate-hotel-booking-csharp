namespace CorporateHotelBooking.Employees.Domain;

public record CompanyId(Guid Value)
{
    public static CompanyId New()
    {
        return From(Guid.NewGuid());
    }

    public static CompanyId From(Guid value)
    {
        return new CompanyId(value);
    }
}