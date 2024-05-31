using Models.Transport.DTO;
using MongoDB.Driver.Linq;
using TransportQuery.Model;
using TransportQuery.Repository.Ticket;
using TransportQuery.Repository.Transport;

namespace TransportQuery.Service.Transport
{
    public class TransportService : ITransportService
    {
        private readonly ITransportRepository _transportRepository;

        private readonly IReservedTicketRepository _reservedTicketRepository;
        public TransportService(ITransportRepository transportRepository, IReservedTicketRepository reservedTicketRepository) {
            _transportRepository = transportRepository;
            _reservedTicketRepository = reservedTicketRepository;
        }
        
        public TransportDTO GetTransportForCriteria(CriteriaForTransport criteria)
        {
            var transportConnections = GenerateTransportConnections(criteria);

            List<FlightDTO> flights = transportConnections
                .Where(pair => pair.Value.ArrivalId != null && pair.Value.DepartureId != null)
                .Select(pair => new FlightDTO
                {
                    Departure = pair.Key,
                    DepartureTransportId = pair.Value.DepartureId.Value,
                    ReturnTransportId = pair.Value.ArrivalId.Value,
                    PricePerSeat = pair.Value.Price
                })
                .ToList();

            var transportDto = new TransportDTO
            {
                Destination = criteria.Departure,
                ChosenFlight = flights.ElementAt(0),
                PossibleFlights = flights
            };
            return transportDto;
        }

        public TransportsDTO GetTransportsForCriteria(CriteriaForTransports criteria)
        {
            var transportConnections = GenerateTransportsConnections(criteria);

            var transports = transportConnections
                .Where(pair => pair.Value.ArrivalId != null && pair.Value.DepartureId != null)
                .Select(pair => new TransportDTO
                {
                    Destination = pair.Key,
                    ChosenFlight = new FlightDTO
                    {
                        Departure = pair.Key,
                        DepartureTransportId = pair.Value.DepartureId.Value,
                        ReturnTransportId = pair.Value.ArrivalId.Value,
                        PricePerSeat = pair.Value.Price
                    },
                    PossibleFlights = new List<FlightDTO>
                    {
                        new FlightDTO
                        {
                            Departure = pair.Key,
                            DepartureTransportId = pair.Value.DepartureId.Value,
                            ReturnTransportId = pair.Value.ArrivalId.Value,
                            PricePerSeat = pair.Value.Price
                        }
                    }
                })
                .ToList();

            return new TransportsDTO { Transports = transports };
        }

        private Dictionary<string, DepartureAndArrivalModel> GenerateTransportConnections(CriteriaForTransport criteria)
        {
            Dictionary<string, DepartureAndArrivalModel> transportConnections = new Dictionary<string, DepartureAndArrivalModel>();

            var destinationFlights = _transportRepository.GetDepartureFlightConnections(criteria.Departure)
                .Where(f => f.ArrivalCountry == criteria.Country && f.ArrivalLocation == criteria.Destination);
            foreach (var destinationFlight in destinationFlights)
            {
                var transportsDF = _transportRepository.GetTransportsById(destinationFlight.Id);

                foreach (var transportDF in transportsDF)
                {
                    int seatsTotal = transportDF.NumberOfSeats;
                    int seatsTaken = _transportRepository.GetNumberOfTakenSeatsForTransport(transportDF.Id);

                    if ((criteria.NumberOfPeople <= (seatsTotal - seatsTaken)) && (criteria.BeginDate >= transportDF.DepartureDate))
                    {
                        if (!transportConnections.ContainsKey(destinationFlight.DepartureLocation))
                        {
                            transportConnections[destinationFlight.DepartureLocation] = new DepartureAndArrivalModel();
                        }

                        transportConnections[destinationFlight.DepartureLocation].DepartureId = transportDF.Id;
                        transportConnections[destinationFlight.DepartureLocation].Price += transportDF.PricePerSeat; 
                        break;
                    }
                }
            }

            var returnFlights = _transportRepository.GetArrivalFlightConnections(criteria.Departure)
                .Where(f => f.ArrivalCountry == criteria.Country && f.ArrivalLocation == criteria.Destination);
            foreach (var returnFlight in returnFlights)
            {
                var transportsRF = _transportRepository.GetTransportsById(returnFlight.Id);

                foreach (var transportRF in transportsRF)
                {
                    int seatsTotal = transportRF.NumberOfSeats;
                    int seatsTaken = _transportRepository.GetNumberOfTakenSeatsForTransport(transportRF.Id);

                    if ((criteria.NumberOfPeople <= (seatsTotal - seatsTaken)) && (criteria.EndDate <= transportRF.DepartureDate))
                    {
                        if (!transportConnections.ContainsKey(returnFlight.ArrivalLocation))
                        {
                            transportConnections[returnFlight.ArrivalLocation] = new DepartureAndArrivalModel();
                        }

                        transportConnections[returnFlight.ArrivalLocation].ArrivalId = transportRF.Id;
                        transportConnections[returnFlight.ArrivalLocation].Price += transportRF.PricePerSeat; 
                        break;
                    }
                }
            }

            return transportConnections;
        }
        
        private List<DepartureAndArrivalModel> GenerateTransportsConnections(CriteriaForTransports criteria)
        {
            List<DepartureAndArrivalModel> transportConnections = new List<DepartureAndArrivalModel>();

            var destinationFlights = _transportRepository.GetDepartureFlightConnections(criteria.Departure)
                .Where(df => df.ArrivalCountry==criteria.Country);
            foreach (var destinationFlight in destinationFlights)
            {
                var transportsDF = _transportRepository.GetTransportsById(destinationFlight.Id);

                foreach (var transportDF in transportsDF)
                {
                    int seatsTotal = transportDF.NumberOfSeats;
                    int seatsTaken = _transportRepository.GetNumberOfTakenSeatsForTransport(transportDF.Id);

                    if ((criteria.NumberOfPeople <= (seatsTotal - seatsTaken)) && (criteria.BeginDate >= transportDF.DepartureDate))
                    {
                        if (!transportConnections.Any(tc => tc.ArrivalLocation == destinationFlight.ArrivalLocation && tc.DepratureLocation == destinationFlight.DepartureLocation))
                        {
                            transportConnections.Add(new  DepartureAndArrivalModel
                            {
                                DepratureLocation = destinationFlight.DepartureLocation, 
                                ArrivalLocation = destinationFlight.ArrivalLocation
                            });
                        }

                        int tcIndex = transportConnections.FindIndex(tc =>
                            tc.ArrivalLocation == destinationFlight.ArrivalLocation &&
                            tc.DepratureLocation == destinationFlight.DepartureLocation);
                        transportConnections[tcIndex].DepartureId = transportDF.Id;
                        transportConnections[tcIndex].Price += transportDF.PricePerSeat; 
                        break;
                    }
                }
            }
            var destinations = destinationFlights.Select(df => df.ArrivalLocation).ToList();
            var otherDestinationFlights = _transportRepository.GetConnectionsForArrivalLocations(destinations)
                .Where(df => df.DepartureLocation != criteria.Departure);
            foreach (var destinationFlight in otherDestinationFlights)
            {
                var transportsDF = _transportRepository.GetTransportsById(destinationFlight.Id);

                foreach (var transportDF in transportsDF)
                {
                    int seatsTotal = transportDF.NumberOfSeats;
                    int seatsTaken = _transportRepository.GetNumberOfTakenSeatsForTransport(transportDF.Id);

                    if ((criteria.NumberOfPeople <= (seatsTotal - seatsTaken)) && (criteria.BeginDate >= transportDF.DepartureDate))
                    {
                        if (!transportConnections.Any(tc => tc.ArrivalLocation == destinationFlight.ArrivalLocation && tc.DepratureLocation == destinationFlight.DepartureLocation))
                        {
                            transportConnections.Add(new  DepartureAndArrivalModel
                            {
                                DepratureLocation = destinationFlight.DepartureLocation, 
                                ArrivalLocation = destinationFlight.ArrivalLocation
                            });
                        }

                        int tcIndex = transportConnections.FindIndex(tc =>
                            tc.ArrivalLocation == destinationFlight.ArrivalLocation &&
                            tc.DepratureLocation == destinationFlight.DepartureLocation);
                        transportConnections[tcIndex].DepartureId = transportDF.Id;
                        transportConnections[tcIndex].Price += transportDF.PricePerSeat; 
                        break;
                    }
                }
            }

            var returnFlights = _transportRepository.GetArrivalFlightConnections(criteria.Departure)
                .Where(rf => destinationFlights.Select(df =>  df.ArrivalLocation).Any(df => df == rf.DepartureLocation));
            foreach (var returnFlight in returnFlights)
            {
                var transportsRF = _transportRepository.GetTransportsById(returnFlight.Id);

                foreach (var transportRF in transportsRF)
                {
                    int seatsTotal = transportRF.NumberOfSeats;
                    int seatsTaken = _transportRepository.GetNumberOfTakenSeatsForTransport(transportRF.Id);

                    if ((criteria.NumberOfPeople <= (seatsTotal - seatsTaken)) &&
                        (criteria.EndDate >= transportRF.DepartureDate))
                    {
                        if (transportConnections.Any(tc =>
                                tc.ArrivalLocation == returnFlight.DepartureLocation &&
                                tc.DepratureLocation == returnFlight.ArrivalLocation))
                        {
                            int tcIndex = transportConnections.FindIndex(tc =>
                                tc.ArrivalLocation == returnFlight.DepartureLocation &&
                                tc.DepratureLocation == returnFlight.ArrivalLocation);
                            transportConnections[tcIndex].ArrivalId = transportRF.Id;
                            transportConnections[tcIndex].Price += transportRF.PricePerSeat;
                            break;
                        }
                    }
                }
            }
            var otherReturnConnections = _transportRepository.GetConnectionsForDepartureLocations(destinations)
                .Where(rc => rc.ArrivalLocation != criteria.Departure);
            foreach (var returnFlight in otherReturnConnections)
            {
                var transportsRF = _transportRepository.GetTransportsById(returnFlight.Id);

                foreach (var transportRF in transportsRF)
                {
                    int seatsTotal = transportRF.NumberOfSeats;
                    int seatsTaken = _transportRepository.GetNumberOfTakenSeatsForTransport(transportRF.Id);

                    if ((criteria.NumberOfPeople <= (seatsTotal - seatsTaken)) &&
                        (criteria.EndDate >= transportRF.DepartureDate))
                    {
                        if (transportConnections.Any(tc =>
                                tc.ArrivalLocation == returnFlight.DepartureLocation &&
                                tc.DepratureLocation == returnFlight.ArrivalLocation))
                        {
                            int tcIndex = transportConnections.FindIndex(tc =>
                                tc.ArrivalLocation == returnFlight.DepartureLocation &&
                                tc.DepratureLocation == returnFlight.ArrivalLocation);
                            transportConnections[tcIndex].ArrivalId = transportRF.Id;
                            transportConnections[tcIndex].Price += transportRF.PricePerSeat;
                            break;
                        }
                    }
                }
            }
            return transportConnections;
        }

        public Task<bool> ReserveTransport(TransportReservationDTO dto, int ArrivalTicketId, int ReturnTicketId)
        {
            return _reservedTicketRepository.ReserveTicketsAsync(dto.ArrivalTransportId, dto.ReturnTransportId, dto.NumberOfPeople,
                dto.OfferId, ArrivalTicketId, ReturnTicketId);
        }

        public Task CancelTransport(int offerId)
        {
            return _reservedTicketRepository.CancelTickets(offerId);
        }
    }
}
