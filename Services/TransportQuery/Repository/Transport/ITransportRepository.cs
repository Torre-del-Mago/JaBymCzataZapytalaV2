using Models.Admin.DTO;
using TransportQuery.Database.Entity;

namespace TransportQuery.Repository.Transport
{
    public interface ITransportRepository
    {
        int GetNumberOfTakenSeatsForTransport(int transportId);
        List<Database.Entity.Transport> GetTransportsById(int flightConnectionId);
        List<FlightConnection> GetDepartureFlightConnections(string departure);
        List<FlightConnection> GetConnectionsForArrivalLocations(List<string> arrivalLocations);
        List<FlightConnection> GetConnectionsForDepartureLocations(List<string> departureLocations);
        List<FlightConnection> GetArrivalFlightConnections(string arrival);

        List<FlightConnection> GetFlightList(string country);
        List<EntryDTO> GetTopDepartures(int numberOfElements);
        List<EntryDTO> GetTopDestinations(int numberOfElements);

        void UpdateNumberOfSeats(int transportId, int numberOfSeats);
        void UpdatePricePerSeat(int transportId, double pricePerSeat);
    }
}
