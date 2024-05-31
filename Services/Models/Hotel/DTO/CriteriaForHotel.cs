namespace Models.Hotel.DTO
{
    public class CriteriaForHotel
    {
        public DateOnly BeginDate { get; set; }

        public DateOnly EndDate { get; set; }

        public int NumberOfPeople { get; set; }

        public int HotelId { get; set; }

    }
}
