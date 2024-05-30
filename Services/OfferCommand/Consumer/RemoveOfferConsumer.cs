﻿using MassTransit;
using Models.Offer;
using OfferCommand.Repository;
using OfferCommand.Repository.EventRepository;
using OfferCommand.Repository.OfferRepository;

namespace OfferCommand.Consumer
{
    public class RemoveOfferConsumer : IConsumer<RemoveOfferEvent>
    {
        private IOfferRepository _offerRepository;
        private IEventRepository _eventRepository;
        private IPublishEndpoint _publishEndpoint;
        public RemoveOfferConsumer(
            IOfferRepository offerRepository,
            IEventRepository eventRepository,
            IPublishEndpoint publishEndpoint)
        {
            _offerRepository = offerRepository;
            _eventRepository = eventRepository;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<RemoveOfferEvent> context)
        {
            _offerRepository.UpdateStatus(context.Message.OfferId, EventTypes.Removed);
            _eventRepository.insertRemovedEvent(context.Message.OfferId);
            await _publishEndpoint.Publish(new RemoveOfferSyncEvent()
            {
                OfferId = context.Message.OfferId
            });
        }
    }
}
