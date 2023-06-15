using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Data.Sql.DataModel;

namespace CorporateHotelBooking.Data.Sql.Mappers;

public class CompanyPolicyDataMapper
{
    public static CompanyPolicyData MapCompanyPolicyDataFrom(CompanyPolicy companyPolicy)
    {
        return new CompanyPolicyData
        {
            CompanyId = companyPolicy.CompanyId.Value,
            RoomTypes = string.Join(";", companyPolicy.RoomTypes)
        };
    }

    public static CompanyPolicy HydrateDomainFrom(CompanyPolicyData companyPolicyData)
    {
        return new CompanyPolicy(CompanyId.From(companyPolicyData.CompanyId), companyPolicyData.RoomTypes.Split(";").ToList());
    }

    public static void ApplyDomainChanges(CompanyPolicyData companyPolicyData, CompanyPolicy companyPolicy)
    {
        companyPolicyData.RoomTypes = string.Join(";", companyPolicy.RoomTypes);
    }
}