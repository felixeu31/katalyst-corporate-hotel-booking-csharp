using System.ComponentModel.DataAnnotations;

namespace CorporateHotelBooking.Data.Sql.DataModel;

public class HotelData
{
    public HotelData()
    {
        Rooms = new HashSet<RoomData>();
    }

    [Key]
    public Guid HotelId { get; set; }

    public string HotelName { get; set; }
    public virtual ICollection<RoomData> Rooms { get; set; }
}