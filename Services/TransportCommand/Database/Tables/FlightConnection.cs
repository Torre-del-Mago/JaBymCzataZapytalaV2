using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportCommand.Database.Tables;

public class FlightConnection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string DepartureLocation { get; set; }
    
    public string ArrivalLocation { get; set; }
}