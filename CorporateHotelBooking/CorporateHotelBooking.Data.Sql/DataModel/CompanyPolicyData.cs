using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateHotelBooking.Data.Sql.DataModel;

public class CompanyPolicyData
{
    [Key]
    public Guid CompanyId { get; set; }

    public string RoomTypes { get; set; }
}