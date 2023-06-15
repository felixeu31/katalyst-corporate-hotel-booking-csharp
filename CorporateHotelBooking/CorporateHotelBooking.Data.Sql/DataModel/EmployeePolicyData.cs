using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateHotelBooking.Data.Sql.DataModel;

public class EmployeePolicyData
{
    [Key]
    public Guid EmployeeId { get; set; }

    public string RoomTypes { get; set; }
}