namespace Models.Transport.DTO
{
    public class CriteriaForTransport
    {
        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NumberOfPeople { get; set; }

        public string Country { get; set; }

        public string DestinationCity { get; set; }

    }
}
