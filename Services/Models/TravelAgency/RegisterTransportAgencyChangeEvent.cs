namespace Models.TravelAgency;

public class RegisterTransportAgencyChangeEvent : EventModel
{
    public string EventName { get; set; }
    public int IdChanged { get; set; }
    public double Change { get; set; }
}