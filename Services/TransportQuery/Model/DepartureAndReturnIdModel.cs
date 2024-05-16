namespace TransportQuery.Model
{
    public class DepartureAndReturnIdModel
    {
        public int DepartureId { get; set; }
        public int ReturnId { get; set; }   
        
        //for both departure and return
        public float Price { get; set; }
    }
}
