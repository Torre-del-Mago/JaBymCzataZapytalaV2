﻿using TransportCommand.Database.Tables;

namespace TransportCommand.Repository.ReservedTicketRepository;

public interface IReservedTicketRepository
{
    Task<List<ReservedTicket>> GetAllReservedTicketsAsync();

    Task<ReservedTicket> GetReservedTicketByIdAsync(int ticketId);

    Task<List<ReservedTicket>> GetReservedTicketsByTransportId(int transportId);

    Task<int> InsertTicket(ReservedTicket ticket);
}