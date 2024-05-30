﻿using MassTransit;
using MassTransit.Transports;
using Models.Transport;
using TransportQuery.Service.Transport;

namespace TransportQuery.Consumer
{
    public class ReserveTransportSyncConsumer : IConsumer<ReserveTransportSyncEvent>
    {
        private readonly ITransportService _service;

        public ReserveTransportSyncConsumer(ITransportService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<ReserveTransportSyncEvent> context)
        {
            _service.ReserveTransport(context.Message.Reservation);
            return Task.CompletedTask;
        }
    }
}
