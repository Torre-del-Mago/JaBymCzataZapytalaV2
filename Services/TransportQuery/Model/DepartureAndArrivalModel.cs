namespace TransportQuery.Model
{
    public class DepartureAndArrivalModel
    {
        public int DepartureId { get; set; }
        public int ArrivalId { get; set; }   
        
        //for both departure and return
        public float Price { get; set; }
    }
}
