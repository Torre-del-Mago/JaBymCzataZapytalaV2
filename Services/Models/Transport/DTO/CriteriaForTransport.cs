namespace Models.Transport.DTO
{
    public class CriteriaForTransport
    {
        public DateOnly BeginDate { get; set; }
        
        public DateOnly EndDate { get; set; }
        
        public int NumberOfPeople { get; set; }

        public string Country { get; set; }
        public string Departure { get; set; }
        
        public string Destination { get; set; }
    }
}
