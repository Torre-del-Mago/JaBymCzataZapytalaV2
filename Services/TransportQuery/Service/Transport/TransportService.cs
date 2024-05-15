using Models.Transport.DTO;
using TransportQuery.DTO;
using TransportQuery.Repository.Transport;

namespace TransportQuery.Service.Transport
{
    public class TransportService : ITransportService
    {
        private ITransportRepository _repository { get; set; }
        public TransportService(ITransportRepository transportRepository) {
            _repository = transportRepository;
        }

        private bool checkIfFlightsHaveEnoughSeats(CriteriaForTransport criteria)
        {
            throw new NotImplementedException();

            /*
             var transports = _repository.GetTransportsByIds(criteria.ChosenFlight.DepartureId, criteria.ChosenFlight.ReturnId);
             foreach(var transport in transports) {
                int takenSeats = _repository.getNumberOfTakenSeatsForTransport(transport.Id);
                int totalSeats = transport.NumberOfSeats;
                if(criteria.ChosenFlight.NumberOfSeats > (totalSeats - takenSeats)) {
                  return false;
            }
            }
            return true;
             */
        }

        public Models.Transport.DTO.TransportDTO getTransportForCriteria(CriteriaForTransport criteria)
        {
            var enoughSeats = checkIfFlightsHaveEnoughSeats(criteria);
            if(!enoughSeats)
            {
                // Jak oznaczyć że dla wybranego transportu nie ma miejsc
                return new Models.Transport.DTO.TransportDTO();
            }

            var departureConnections = _repository.getConnectionGoingTo(criteria.DestinationCity);
            var returnConnections = _repository.getConnectionComingFrom(criteria.DestinationCity);

            Dictionary<string, DepartureAndReturnIdDTO> idsForConnection = new Dictionary<string, DepartureAndReturnIdDTO>();

            foreach (ConnectionDTO connection in departureConnections)
            {
                var transports = _repository.getTransportsForConnections(connection.Id);
                foreach(var transport in transports)
                {
                    int numberOfSeatsTotal = transport.NumberOfSeats;
                    int numberOfSeatsTaken = _repository.getNumberOfTakenSeatsForTransport(transport.Id);
                    if(criteria.NumberOfPeople <= (numberOfSeatsTotal - numberOfSeatsTaken))
                    {
                        idsForConnection[connection.LocationName].DepartureId = transport.Id;
                        idsForConnection[connection.LocationName].Price += transport.Price;
                        break;
                    }
                }
            }

            foreach (ConnectionDTO connection in returnConnections)
            {
                var transports = _repository.getTransportsForConnections(connection.Id);
                foreach (var transport in transports)
                {
                    int numberOfSeatsTotal = transport.NumberOfSeats;
                    int numberOfSeatsTaken = _repository.getNumberOfTakenSeatsForTransport(transport.Id);
                    if (criteria.NumberOfPeople <= (numberOfSeatsTotal - numberOfSeatsTaken))
                    {
                        idsForConnection[connection.LocationName].ReturnId = transport.Id;
                        idsForConnection[connection.LocationName].Price += transport.Price;
                        break;
                    }
                }
            }

            List<FlightDTO> flights = new List<FlightDTO>();

            foreach(KeyValuePair<string, DepartureAndReturnIdDTO> pair in idsForConnection)
            {
                if(pair.Value.ReturnId != null && pair.Value.DepartureId != null)
                {
                    flights.Add(new FlightDTO()
                    {
                        Departure = pair.Key,
                        DepartureTransportId = pair.Value.DepartureId,
                        ReturnTransportId = pair.Value.ReturnId,
                        PricePerSeat = pair.Value.Price
                    });
                }
            }

            var result = new Models.Transport.DTO.TransportDTO();
            result.PossibleFlights = flights;
            result.ChosenFlight = flights.ElementAt(0);
            result.Destination = criteria.DestinationCity;

            return result;

            /*
             Fetch going to destination
             Fetch coming from destination
             See which ones have available number of seats
             Get first with enough seats
             */
        }
    }
}
