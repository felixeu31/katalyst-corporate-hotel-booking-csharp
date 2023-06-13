using System.ComponentModel.DataAnnotations;

namespace CorporateHotelBooking.Data.Sql.DataModel;

public class EmployeeData
{
    [Key]
    public Guid EmployeeId { get; set; }
    public Guid CompanyId { get; set; }
}