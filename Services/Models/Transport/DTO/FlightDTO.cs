using System.Security.Cryptography.X509Certificates;

namespace Models.Transport.DTO
{
    public class FlightDTO
    {

        public int DepartureTransportId { get; set; }

        public int ReturnTransportId { get; set; }

        // it's a sum of depature price and return price
        public float PricePerSeat { get; set; }

        public string Departure { get; set; }

    }
}
