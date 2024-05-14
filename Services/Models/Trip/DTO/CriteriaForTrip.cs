namespace Models.Trip.DTO
{
    public class CriteriaForTrip
    {
        public int HotelId { get; set; }

        public int NrOfPeople { get; set; }

        public String Country { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }


    }
}
