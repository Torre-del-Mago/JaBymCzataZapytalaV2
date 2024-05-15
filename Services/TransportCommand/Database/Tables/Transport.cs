﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportCommand.Database.Tables;

public class Transport
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    
    public string ConnectionId { get; set; }
    
    public int NumberOfSeats { get; set; }
    
    public DateTime DepartureDate { get; set; }
    
    public decimal PricePerSeat { get; set; }
}