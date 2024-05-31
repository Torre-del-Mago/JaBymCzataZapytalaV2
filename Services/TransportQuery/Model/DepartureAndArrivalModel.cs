namespace TransportQuery.Model
{
    public class DepartureAndArrivalModel
    {
        public string? DepratureLocation { get; set; }
        public int? DepartureId { get; set; }
        
        public string? ArrivalLocation { get; set; }
        public int? ArrivalId { get; set; }   
        
        //for both departure and return
        public float Price { get; set; }
    }
}
