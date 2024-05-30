﻿using MassTransit;
using Models.Offer;
using OfferQuery.Service;

namespace OfferQuery.Consumer
{
    public class PaidOfferSyncConsumer : IConsumer<PaidOfferSyncEvent>
    {
        private readonly IOfferService _service;

        public PaidOfferSyncConsumer(IOfferService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<PaidOfferSyncEvent> context)
        {
            return _service.SynchroniseContent(context.Message.OfferSync, context.Message.RoomSyncs);
        }
    }
}
