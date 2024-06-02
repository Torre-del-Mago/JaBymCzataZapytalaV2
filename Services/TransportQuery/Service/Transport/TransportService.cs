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
            var transports = new List<TransportDTO>();
            var transportConnections = GenerateTransportsConnections(new CriteriaForTransports()
            {
                BeginDate = criteria.BeginDate,
                EndDate = criteria.EndDate,
                NumberOfPeople = criteria.NumberOfPeople,
                Country = criteria.Country,
                Departure = criteria.Departure
            });

            var criteriaDestinationTransportConnections =
                transportConnections.Where(tc => tc.DepratureLocation != null && tc.DepratureLocation == criteria.Departure).ToList();
            var destinations = transportConnections.Where(tc => tc.ArrivalLocation ==  criteria.Destination)
                .Select(tc => tc.ArrivalLocation).Distinct().ToList();
            foreach (var destination in destinations)
            {
                var transport = new TransportDTO();
                int index = criteriaDestinationTransportConnections.FindIndex(tc => tc.ArrivalLocation == destination);
                transport.Destination = destination;
                transport.ChosenFlight = new FlightDTO
                {
                    DepartureTransportId = criteriaDestinationTransportConnections[index].DepartureId.Value,
                    ReturnTransportId = criteriaDestinationTransportConnections[index].ArrivalId.Value,
                    PricePerSeat = criteriaDestinationTransportConnections[index].Price,
                    Departure = criteriaDestinationTransportConnections[index].DepratureLocation
                };
                transport.PossibleFlights = transportConnections
                    .Where(tc => tc.ArrivalLocation == destination)
                    .Select(tc => new FlightDTO()
                    {
                        DepartureTransportId = tc.DepartureId.Value,
                        ReturnTransportId = tc.ArrivalId.Value,
                        PricePerSeat = tc.Price,
                        Departure = tc.DepratureLocation
                    }).ToList();
                transports.Add(transport);
            }
            return transports.Count >0 ? transports[0] : new TransportDTO();
        }

        public TransportsDTO GetTransportsForCriteria(CriteriaForTransports criteria)
        {
            var transports = new List<TransportDTO>();
            var transportConnections = GenerateTransportsConnections(criteria);
            var criteriaDestinationTransportConnections =
                transportConnections.Where(tc => tc.DepratureLocation != null && tc.DepratureLocation == criteria.Departure).ToList();
            var destinations = transportConnections.Select(tc => tc.ArrivalLocation).Distinct().ToList();
            foreach (var destination in destinations)
            {
                var transport = new TransportDTO();
                int index = criteriaDestinationTransportConnections.FindIndex(tc => tc.ArrivalLocation == destination);
                transport.Destination = destination;
                transport.ChosenFlight = new FlightDTO
                {
                    DepartureTransportId = criteriaDestinationTransportConnections[index].DepartureId.Value,
                    ReturnTransportId = criteriaDestinationTransportConnections[index].ArrivalId.Value,
                    PricePerSeat = criteriaDestinationTransportConnections[index].Price,
                    Departure = criteriaDestinationTransportConnections[index].DepratureLocation
                };
                transport.PossibleFlights = transportConnections
                    .Where(tc => tc.ArrivalLocation == destination)
                    .Select(tc => new FlightDTO()
                    {
                        DepartureTransportId = tc.DepartureId.Value,
                        ReturnTransportId = tc.ArrivalId.Value,
                        PricePerSeat = tc.Price,
                        Departure = tc.DepratureLocation
                    }).ToList();
                transports.Add(transport);
            }
            
            return new TransportsDTO { Transports = transports };
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
