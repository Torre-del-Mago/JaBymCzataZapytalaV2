namespace Models.Transport.DTO
{
    public class TransportDTO
    {
        public string Destination { get; set; }

        public FlightDTO ChosenFlight { get; set; }

        public List<FlightDTO> PossibleFlights { get; set; }

    }
}
