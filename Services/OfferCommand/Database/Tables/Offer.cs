﻿using OfferCommand.Database.Tables;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfferQuery.Database.Entity;

public class Offer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string DepartureTicketId { get; set; }
    
    public string ArrivalTicketId { get; set; }
    
    public int DepartureTransportId { get; set; }
    
    public int ArrivalTransportId { get; set; }
    
    public DateTime DateFrom { get; set; }
    
    public DateTime DateTo { get; set; }
    
    public int HotelId { get; set; }
    
    public int NumberOfAdults { get; set; }
    
    public int NumberOfNewborns { get; set; }
    
    public int NumberOfToddlers { get; set; }
    
    public int NumberOfTeenagers { get; set; }
    
    public string UserLogin { get; set; }
    
    public string OfferStatus { get; set; }
}