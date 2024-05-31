using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportCommand.Database.Tables;

public class Transport
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int ConnectionId { get; set; }
    
    public int NumberOfSeats { get; set; }
    
    public DateOnly DepartureDate { get; set; }
    
    public decimal PricePerSeat { get; set; }
}