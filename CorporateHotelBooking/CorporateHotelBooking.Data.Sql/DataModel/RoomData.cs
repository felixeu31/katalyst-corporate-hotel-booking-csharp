using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateHotelBooking.Data.Sql.DataModel;

public class RoomData
{
    public int RoomNumber { get; set; }

    public string RoomType { get; set; }

    public Guid HotelId { get; set; }

    [ForeignKey(nameof(HotelId))]
    public virtual HotelData Hotel { get; set; }
}