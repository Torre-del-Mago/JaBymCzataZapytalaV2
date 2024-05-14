namespace Models.Trip.DTO
{
    public class TripDTO
    {
        public String HotelName { get; set; }

        public String Country { get; set; }

        public String City { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<String> TypesOfMeals { get; set; }

        // TODO lista pokoi



    }
}
