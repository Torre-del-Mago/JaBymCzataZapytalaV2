using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportCommand.Database.Tables;

public class ReservedTicket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    
    public string TransportId { get; set; }
    
    public int NumberOfReservedSeats { get; set; }
}