namespace TransportQuery.Model
{
    public class DepartureAndReturnIdModel
    {
        public string DepartureId { get; set; }
        public string ReturnId { get; set; }   
        
        //for both departure and return
        public float Price { get; set; }
    }
}
