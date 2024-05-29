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
            var events = await _eventRepository.getAllTicketEventsForTransportId(transportId);
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

        public async Task<bool> reserveTransport(TransportReservationDTO dto)
        {
            var activeArrivalTicketsTask = getAllActiveTicketsForTransportId(dto.ArrivalTransportId);
            var activeReturnTicketsTask = getAllActiveTicketsForTransportId(dto.ReturnTransportId);

            var arrivalTransportTask = _transportRepository.GetTransportByIdAsync(dto.ArrivalTransportId);

            var returnTransportTask = _transportRepository.GetTransportByIdAsync(dto.ReturnTransportId);
            var arrivalTicketsTask = _ticketRepository.GetReservedTicketsByTransportId(dto.ArrivalTransportId);
            var returnTicketsTask = _ticketRepository.GetReservedTicketsByTransportId(dto.ReturnTransportId);

            var arrivalTickets = await arrivalTicketsTask;
            var activeArrivalTickets = await activeArrivalTicketsTask;

            var arrivalTicketsSum = arrivalTickets.Where(t => activeArrivalTickets.Contains(t.Id)).Select(t => t.NumberOfReservedSeats).Sum();

            var returnTickets = await returnTicketsTask;
            var activeReturnTickets = await activeArrivalTicketsTask;

            var returnTicketsSum = returnTickets.Where(t => activeReturnTickets.Contains(t.Id)).Select(t => t.NumberOfReservedSeats).Sum();

            var arrivalTransport = await arrivalTransportTask;
            var returnTransport = await returnTransportTask;

            var canBoardArrivalTransport = arrivalTicketsSum + dto.NumberOfPeople <= arrivalTransport.NumberOfSeats;
            var canBoardReturnTransport = returnTicketsSum + dto.NumberOfPeople <= returnTransport.NumberOfSeats;

            var canReserveTransport = canBoardArrivalTransport && canBoardReturnTransport;

            if(!canReserveTransport)
            {
                return false;
            }

            ReservedTicket arrivalTicket = new ReservedTicket()
            {
                NumberOfReservedSeats = dto.NumberOfPeople,
                TransportId = dto.ArrivalTransportId
            };
            ReservedTicket returnTicket = new ReservedTicket()
            {
                NumberOfReservedSeats = dto.NumberOfPeople,
                TransportId = dto.ReturnTransportId
            };
            var arrivalTicketId = await _ticketRepository.insertTicket(arrivalTicket);
            var returnTicketId = await _ticketRepository.insertTicket(returnTicket);

            await _eventRepository.insertReservationEvent(dto.ReturnTransportId, arrivalTicketId);
            await _eventRepository.insertReservationEvent(dto.ReturnTransportId, returnTicketId);

            return true;
            
        }

        public async Task cancelTransport(int arrivalTicketId, int returnTicketId)
        {
            var arrivalTicket = await _ticketRepository.GetReservedTicketByIdAsync(arrivalTicketId);
            var returnTicket = await _ticketRepository.GetReservedTicketByIdAsync(returnTicketId);

            await _eventRepository.insertCancellationEvent(arrivalTicket.Id, arrivalTicketId);
            await _eventRepository.insertCancellationEvent(returnTicket.Id, returnTicketId); 
        }
    }
}
