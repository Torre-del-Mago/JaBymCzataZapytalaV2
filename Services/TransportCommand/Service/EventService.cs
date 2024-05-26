using Models.Transport.DTO;
using TransportCommand.Database.Tables;
using TransportCommand.Repository.ReservedTicketRepository;
using TransportCommand.Repository.TransportRepository;

namespace TransportCommand.Service
{
    public class EventService : IEventService
    {
        private IReservedTicketRepository _ticketRepository;
        private ITransportRepository _transportRepository;
        public EventService(IReservedTicketRepository ticketRepository, ITransportRepository transportRepository)
        {
            _ticketRepository = ticketRepository;
            _transportRepository = transportRepository;
        }

        public async Task<bool> reserveTransport(TransportReservationDTO dto)
        {
            var arrivalTransportTask = _transportRepository.GetTransportByIdAsync(dto.ArrivalTransportId);
            var returnTransportTask = _transportRepository.GetTransportByIdAsync(dto.ReturnTransportId);
            var arrivalTicketsTask = _ticketRepository.GetReservedTicketsByTransportId(dto.ArrivalTransportId);
            var returnTicketsTask = _ticketRepository.GetReservedTicketsByTransportId(dto.ReturnTransportId);

            var arrivalTickets = await arrivalTicketsTask;
            var returnTickets = await returnTicketsTask;

            var arrivalTicketsSum = arrivalTickets.Select(t => t.NumberOfReservedSeats).Sum();
            var returnTicketsSum = returnTickets.Select(t => t.NumberOfReservedSeats).Sum();

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
            await _ticketRepository.insertTicket(arrivalTicket);
            await _ticketRepository.insertTicket(returnTicket);

            return true;
            
        }
    }
}
