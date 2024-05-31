namespace Models.Trip.DTO
{
    public class CriteriaForTrip
    {
        public int HotelId { get; set; }

        public int NrOfPeople { get; set; }

        public string Country { get; set; }

        public DateOnly BeginDate { get; set; }

        public DateOnly EndDate { get; set; }
        
        public string Departure { get; set; }
        
        public string Destination { get; set; }

    }
}
