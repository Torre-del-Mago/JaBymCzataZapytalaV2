﻿namespace Models.Hotel.DTO
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public string TypeOfRoom { get; set; }

        public int NumberOfPeopleForTheRoom  { get; set; }

        public float PricePerRoom { get; set; }

    }
}
