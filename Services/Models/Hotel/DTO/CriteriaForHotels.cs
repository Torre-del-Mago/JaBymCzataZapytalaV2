namespace Models.Hotel.DTO
{
    public class CriteriaForHotels
    {
        public DateOnly BeginDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int NumberOfPeople { get; set; }

        public string Country {  get; set; }

    }
}
