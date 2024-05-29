﻿using Models.Transport.DTO;

namespace TransportCommand.Service
{
    public interface IEventService
    {
        public Task<bool> reserveTransport(TransportReservationDTO dto);

        public Task cancelTransport(int arrivalTicketId, int returnTicketId);
    }
}