﻿using HotelCommand.Repository.ReservationEventRepository;
using HotelCommand.Service;
using MassTransit;
using Models.Hotel;

namespace HotelCommand.Consumer
{
    public class ReserveHotelConsumer(IEventService eventService, IPublishEndpoint publishEndpoint)
        : IConsumer<ReserveHotelEvent>
    {
        public async Task Consume(ConsumeContext<ReserveHotelEvent> context)
        {
            var hasReservedHotel= await eventService.reserveHotel(context.Message.Reservation);
            if (!hasReservedHotel) {
                await publishEndpoint.Publish(new ReserveHotelEventReply()
                {
                    Answer = ReserveHotelEventReply.State.NOT_RESERVED,
                    CorrelationId = context.Message.CorrelationId
                });            
            }
            await publishEndpoint.Publish(new ReserveHotelEventReply()
            {
                Answer = ReserveHotelEventReply.State.RESERVED,
                CorrelationId = context.Message.CorrelationId
            });

            await publishEndpoint.Publish(new ReserveHotelSyncEvent()
            {
                Reservation = context.Message.Reservation
            });

        }
    }
}
