namespace Models.TravelAgency
{
    public class AddDietEvent : EventModel
    {
        public int HotelId { get; set; }
        public int DietId { get; set; }
    }
}