using Models.Transport.DTO;
using TransportCommand.Database;
using TransportCommand.Database.Tables;
using TransportCommand.Repository.EventRepository;
using TransportCommand.Repository.ReservedTicketRepository;
using TransportCommand.Repository.TransportRepository;

namespace TransportCommand.Service
{
    public class EventService : IEventService
    {
        private IReservedTicketRepository _ticketRepository;
        private ITransportRepository _transportRepository;
        private IEventRepository _eventRepository;
        public EventService(IReservedTicketRepository ticketRepository, ITransportRepository transportRepository,
            IEventRepository eventRepository)
        {
            _ticketRepository = ticketRepository;
            _transportRepository = transportRepository;
            _eventRepository = eventRepository;
        }

        private async Task<List<int>> getAllActiveTicketsForTransportId(int transportId)
        {
            var result = new List<int>();
            var events = await _eventRepository.GetAllTicketEventsForTransportId(transportId);
            foreach(Event e in events)
            {
                if(e.EventType == EventType.Created)
                {
                    result.Add(e.TicketId);
                }
                else if (e.EventType == EventType.Deleted)
                {
                    result.Remove(e.TicketId);
                }
            }
            return result;
        }

        public async Task<bool> ReserveTransport(TransportReservationDTO dto)
        {
            var activeArrivalTickets  = await getAllActiveTicketsForTransportId(dto.ArrivalTransportId);
            var activeReturnTickets = await getAllActiveTicketsForTransportId(dto.ReturnTransportId);

            var arrivalTransport = await _transportRepository.GetTransportByIdAsync(dto.ArrivalTransportId);

            var returnTransport = await _transportRepository.GetTransportByIdAsync(dto.ReturnTransportId);
            var arrivalTickets = await _ticketRepository.GetReservedTicketsByTransportId(dto.ArrivalTransportId);
            var returnTickets = await _ticketRepository.GetReservedTicketsByTransportId(dto.ReturnTransportId);
            
            var arrivalTicketsSum = arrivalTickets.Where(t => activeArrivalTickets.Contains(t.Id)).Select(t => t.NumberOfReservedSeats).Sum();

            Console.Out.WriteLine($"Sum for arrival tickets for offer with id {dto.OfferId} is {arrivalTicketsSum}");
            
            var returnTicketsSum = returnTickets.Where(t => activeReturnTickets.Contains(t.Id)).Select(t => t.NumberOfReservedSeats).Sum();

            Console.Out.WriteLine($"Sum for arrival tickets for offer with id {dto.OfferId} is {returnTicketsSum}");
            
            var canBoardArrivalTransport = arrivalTicketsSum + dto.NumberOfPeople <= arrivalTransport.NumberOfSeats;
            var canBoardReturnTransport = returnTicketsSum + dto.NumberOfPeople <= returnTransport.NumberOfSeats;

            var canReserveTransport = canBoardArrivalTransport && canBoardReturnTransport;

            Console.Out.WriteLine($"Reservation for offer id {dto.OfferId} arrival: {canBoardArrivalTransport} return: {canBoardReturnTransport}");

            if(!canReserveTransport)
            {
                return false;
            }

            ReservedTicket arrivalTicket = new ReservedTicket()
            {
                NumberOfReservedSeats = dto.NumberOfPeople,
                TransportId = dto.ArrivalTransportId,
                OfferId = dto.OfferId
            };
            ReservedTicket returnTicket = new ReservedTicket()
            {
                NumberOfReservedSeats = dto.NumberOfPeople,
                TransportId = dto.ReturnTransportId,
                OfferId = dto.OfferId
            };
            await _ticketRepository.InsertTicket(arrivalTicket);
            await _ticketRepository.InsertTicket(returnTicket);

            await _eventRepository.InsertReservationEvent(arrivalTicket);
            await _eventRepository.InsertReservationEvent(returnTicket);

            return true;
            
        }

        public async Task CancelTransport(int offerId)
        {

            var tickets = await _ticketRepository.GetReservedTicketsByOfferId(offerId);

            await _eventRepository.InsertCancellationEventForTickets(tickets);
        }
    }
}
