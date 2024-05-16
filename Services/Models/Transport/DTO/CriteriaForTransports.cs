namespace Models.Transport.DTO
{
    public class CriteriaForTransports
    {
        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NumberOfPeople { get; set; }

        public string Country { get; set; }
        
        public string Departure { get; set; }
    }
}
