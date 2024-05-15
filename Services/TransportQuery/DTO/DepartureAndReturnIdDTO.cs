namespace TransportQuery.DTO
{
    public class DepartureAndReturnIdDTO
    {
        public string DepartureId { get; set; }
        public string ReturnId { get; set; }   
        
        //for both departure and return
        public float Price { get; set; }
    }
}
