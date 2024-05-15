using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelQuery.Database.Tables;

public class ReservedRoom
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public Guid ReservationId { get; set; }
    public Guid HotelRoomTypesId { get; set; }
    public int NumberOfRooms { get; set; }
}