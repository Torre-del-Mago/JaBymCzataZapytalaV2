﻿using HotelQuery.Service.Hotel;
using MassTransit;
using Models.Hotel;

namespace HotelQuery.Consumer
{
    public class CancelReservationHotelSyncConsumer : IConsumer<CancelReservationHotelSyncEvent>
    {
        private readonly IHotelService _service;
        public CancelReservationHotelSyncConsumer(IHotelService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<CancelReservationHotelSyncEvent> context)
        {
            Console.Out.WriteLine("Hotel Gets Event CancelReservationHotelSyncEvent");
            return _service.CancelHotel(context.Message.OfferId);
        }
    }
}
