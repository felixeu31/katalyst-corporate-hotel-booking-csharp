using System.ComponentModel.DataAnnotations;

namespace CorporateHotelBooking.Data.Sql.DataModel;

public class BookingData
{
    [Key]
    public Guid BookingId { get; set; }
    public Guid HotelId { get; set; }
    public Guid BookedBy { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public string RoomType { get; set; }
    public int RoomNumber { get; set; }
}