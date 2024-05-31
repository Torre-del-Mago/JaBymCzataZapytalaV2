using MassTransit;
using Models.Hotel;
using Models.Offer;
using Models.Offer.DTO;
using Models.Payment;
using Models.Transport;

namespace OfferCommand
{
    public class PaymentTimeout { 
        public Guid CorrelationId { get; set; }
    }

    public class OfferReservation : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }

        public int OfferId { get; set; }

        public OfferDTO Offer { get; set; }

        public bool MadeReservation {  get; set; }
        public bool MadeHotelReservation {  get; set; }
        public bool MadeTransportReservation {  get; set; }
        public int TransportReservationId { get; set; } = 0;
        public int HotelReservationId { get; set; } = 0;
        public bool PaidForReservation {  get; set; }

        public Guid? PaymentTimeoutId { get; set; }

    }
    public class OfferSaga : MassTransitStateMachine<OfferReservation>
    {
        public State WaitingForHotel { get; set; }
        public State WaitingForTransport { get; set; }
        public State ReservedOffer { get; set; }

        private IPublishEndpoint _publishEndpoint { get; set; }

        public Event<CreatedOfferEvent> CreatedOfferEvent { get; set; }
        public Event<CheckPaymentEventReply> PaymentEvent { get; set; }
        public Event<ReserveHotelEventReply> ReserveHotelEvent { get; set; }
        public Event<ReserveTransportEventReply> ReserveTransportEvent { get; set; }

        public Schedule<OfferReservation, PaymentTimeout> PaymentNotSentTimeout { get; set; }

        private void cancelReservations(OfferReservation reservation)
        {
            Console.Out.WriteLine($"Cancelling reservations for saga {reservation.OfferId}");
            if(reservation.MadeTransportReservation)
            {
                _publishEndpoint.Publish(new CancelReservationTransportEvent()
                {
                    CorrelationId = reservation.CorrelationId,
                    OfferId = reservation.OfferId
                });
            }
            if(reservation.MadeHotelReservation)
            {
                _publishEndpoint.Publish(new CancelReservationHotelEvent()
                {
                    CorrelationId = reservation.CorrelationId,
                    OfferId = reservation.OfferId
                });
            }
        }

        private void processHotel(ReserveHotelEventReply reply, OfferReservation reservation)
        {
            reservation.MadeHotelReservation = reply.Answer == ReserveHotelEventReply.State.RESERVED;
        }

        private void processTransport(ReserveTransportEventReply reply, OfferReservation reservation)
        {
            reservation.MadeTransportReservation = reply.Answer == ReserveTransportEventReply.State.RESERVED;
        }

        public OfferSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => CreatedOfferEvent, x => x.SelectId(ctx => ctx.Message.CorrelationId));
            Event(() => PaymentEvent, x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
            Event(() => ReserveHotelEvent, x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
            Event(() => ReserveTransportEvent, x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

            Schedule(() => PaymentNotSentTimeout, instance => instance.PaymentTimeoutId, s =>
            {
                s.Delay = TimeSpan.FromSeconds(70);

                s.Received = r => r.CorrelateById(context => context.Message.CorrelationId);
            });

            Initially(
                When(CreatedOfferEvent).
                Then(ctx => ctx.Saga.OfferId = ctx.Message.OfferId).
                Then(ctx => ctx.Saga.Offer = ctx.Message.Offer).
                Then(ctx => Console.WriteLine($"\nSaga correlation id {ctx.Saga.CorrelationId}, saga consumer correlation id {ctx.Message.CorrelationId}")).
                Then(ctx => Console.WriteLine($"Created Saga with id {ctx.Saga.OfferId}")).
                Publish(ctx => new ReserveHotelEvent()
                {
                    CorrelationId = ctx.Saga.CorrelationId,
                    Reservation = new Models.Hotel.DTO.HotelReservationDTO()
                    {
                        HotelId = ctx.Saga.Offer.HotelId,
                        BeginDate = ctx.Saga.Offer.BeginDate,
                        EndDate = ctx.Saga.Offer.EndDate,
                        Rooms = ctx.Saga.Offer.Rooms,
                        OfferId = ctx.Saga.OfferId
                    }
                }).
                TransitionTo(WaitingForHotel)
                );

            During(WaitingForHotel,
                When(ReserveHotelEvent).
                Then(ctx => processHotel(ctx.Message, ctx.Saga)).
                IfElse(ctx => ctx.Saga.MadeHotelReservation,
                valid => valid.ThenAsync(ctx => Console.Out.WriteLineAsync($"ReservedHotel for Saga with id {ctx.Saga.OfferId}")).
                Publish(ctx => new ReserveTransportEvent() { 
                    CorrelationId = ctx.Saga.CorrelationId,
                    Reservation = new Models.Transport.DTO.TransportReservationDTO()
                    {
                        ArrivalTransportId = ctx.Saga.Offer.Flight.DepartureTransportId,
                        ReturnTransportId = ctx.Saga.Offer.Flight.ReturnTransportId,
                        NumberOfPeople = ctx.Saga.Offer.NumberOfAdults + ctx.Saga.Offer.NumberOfNewborns + 
                        ctx.Saga.Offer.NumberOfTeenagers + ctx.Saga.Offer.NumberOfToddlers,
                        OfferId = ctx.Saga.OfferId
                    }
                }).TransitionTo(WaitingForTransport),
                invalid => invalid.ThenAsync(ctx => Console.Out.WriteLineAsync($"Unable to reserve hotel for Saga with id {ctx.Saga.OfferId}")).
                Respond(ctx => new CreatedOfferEventReply()
                {
                    Answer = CreatedOfferEventReply.State.NOT_RESERVED,
                    CorrelationId = ctx.Saga.CorrelationId,
                    Error = "Could not reserve hotel"
                }
                ).Finalize()));

            During(WaitingForTransport,
                When(ReserveTransportEvent).
                Then(ctx => processTransport(ctx.Message, ctx.Saga)).
                IfElse(ctx => ctx.Saga.MadeTransportReservation,
                valid => valid.Then(ctx => Console.Out.WriteLine($"Reserved Transport for Saga with id {ctx.Saga.OfferId}")).
                Respond(ctx => new CreatedOfferEventReply()
                {
                    Answer = CreatedOfferEventReply.State.RESERVED,
                    CorrelationId = ctx.Saga.CorrelationId
                })
                .Publish(ctx => new CheckPaymentEvent()
                {
                    CorrelationId = ctx.Saga.CorrelationId,
                    OfferId = ctx.Saga.OfferId,
                    TimeForPayment = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                    
                })
                .Schedule(PaymentNotSentTimeout, context => context.Init<PaymentTimeout>(new PaymentTimeout (){ CorrelationId = context.Saga.CorrelationId}))
                .TransitionTo(ReservedOffer),
            invalid => invalid.Then(ctx => Console.Out.WriteLine($"Unable to reserve transport for Saga with id {ctx.Saga.OfferId}"))
                .Publish(ctx => new CancelReservationHotelEvent()
                {
                    CorrelationId = ctx.Saga.CorrelationId,
                    OfferId = ctx.Saga.OfferId
                })
                .Respond(ctx => new CreatedOfferEventReply()
                {
                    Answer = CreatedOfferEventReply.State.NOT_RESERVED,
                    CorrelationId = ctx.Saga.CorrelationId,
                    Error = "Could not reserve transport",
                })
                .Finalize()));


            During(ReservedOffer,
                When(PaymentNotSentTimeout.Received).
                ThenAsync(ctx => Console.Out.WriteLineAsync($"Timeout for Saga with id {ctx.Saga.OfferId}")).
                Publish(ctx => new RemoveOfferEvent()
                {
                    OfferId = ctx.Saga.OfferId
                })
                .Publish(ctx => new CancelReservationHotelEvent()
                {
                    CorrelationId = ctx.Saga.CorrelationId,
                    OfferId = ctx.Saga.OfferId
                })
                .Publish(ctx => new CancelReservationTransportEvent()
                {
                    CorrelationId = ctx.Saga.CorrelationId,
                    OfferId = ctx.Saga.OfferId
                }).
                Finalize(),
                When(PaymentEvent).
                Unschedule(PaymentNotSentTimeout).
                Finalize());
        }
    }
}
