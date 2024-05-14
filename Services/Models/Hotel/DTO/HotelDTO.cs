namespace Models.Hotel.DTO
{
    public class HotelDTO
    {
        public int HotelId { get; set; }

        public string HotelName { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<string> TypesOfMeals { get; set; }

        public float Discount { get; set; }

        public List<RoomDTO> Rooms  { get; set; }

    }
}
