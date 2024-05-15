using System.Security.Cryptography.X509Certificates;

namespace Models.Transport.DTO
{
    public class FlightDTO
    {

        public string DepartureTransportId { get; set; }

        public string ReturnTransportId { get; set; }

        // it's a sum of depature price and return price
        public float PricePerSeat { get; set; }

        public string Departure { get; set; }

    }
}
