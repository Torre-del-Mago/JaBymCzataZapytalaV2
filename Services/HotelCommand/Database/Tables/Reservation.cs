using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelCommand.Database.Tables;

public class Reservation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public Guid HotelId { get; set; }
    
    public DateOnly  From  { get; set; }
    
    public DateOnly  To { get; set; }
    
    public List<ReservedRoom> Rooms { get; set; }
    
}