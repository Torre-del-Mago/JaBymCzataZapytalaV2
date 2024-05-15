using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelQuery.Database.Tables;

public class Reservation
{
    [Key]
    public Guid HotelId { get; set; }
    
    public DateOnly  From  { get; set; }
    
    public DateOnly  To { get; set; }
    
    public List<ReservedRoom> Rooms { get; set; }
    
}