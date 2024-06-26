﻿using MassTransit;
using Models.Transport;
using TransportCommand.Service;

namespace TransportCommand.Consumer
{
    public class CancelReservationTransportConsumer : IConsumer<CancelReservationTransportEvent>
    {
        private IEventService _eventService;
        private IPublishEndpoint _publishEndpoint;
        public CancelReservationTransportConsumer(IEventService eventService, IPublishEndpoint publishEndpoint)
        {
            _eventService = eventService;
            _publishEndpoint = publishEndpoint; 
        }

        public async Task Consume(ConsumeContext<CancelReservationTransportEvent> context)
        {
            Console.Out.WriteLine($"Started cancelling transport for offer with id {context.Message.OfferId}");

            await _eventService.CancelTransport(context.Message.OfferId);

            await _publishEndpoint.Publish(new CancelReservationTransportSyncEvent()
            {
                OfferId = context.Message.OfferId
            }) ;

        }
    }
}
