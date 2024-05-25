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

        public int Registration { get; set; }

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

        public Event<ReserveOfferEvent> ReserveOfferEvent { get; set; }
        public Event<CheckPaymentEventReply> PaymentEvent { get; set; }
        public Event<ReserveHotelEventReply> ReserveHotelEvent { get; set; }
        public Event<ReserveTransportEventReply> ReserveTransportEvent { get; set; }

        public Schedule<OfferReservation, PaymentTimeout> PaymentNotSentTimeout { get; set; }

        private void processPayment()
        {

        }

        private void cancelReservations(OfferReservation reservation)
        {
            if(reservation.TransportReservationId != 0)
            {
                _publishEndpoint.Publish(new CancelReservationTransportEvent()
                {
                    CorrelationId = reservation.CorrelationId,
                    OfferId = reservation.TransportReservationId
                });
            }
            if(reservation.HotelReservationId != 0)
            {
                _publishEndpoint.Publish(new CancelReservationHotelEvent()
                {
                    CorrelationId = reservation.CorrelationId,
                    OfferId = reservation.HotelReservationId
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

        private void sendPaymentMessage()
        {

        }

        private void publishReserveOfferResponse()
        {

        }

        public OfferSaga() 
        {
            InstanceState(x => x.CurrentState);

            Event(() => ReserveOfferEvent, x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
            Event(() => PaymentEvent, x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
            Event(() => ReserveHotelEvent, x => x.CorrelateById(ctx => ctx.Message.CorrelationId));
            Event(() => ReserveTransportEvent, x => x.CorrelateById(ctx => ctx.Message.CorrelationId));

            Schedule(() => PaymentNotSentTimeout, instance => instance.PaymentTimeoutId, s =>
            {
                s.Delay = TimeSpan.FromSeconds(70);

                s.Received = r => r.CorrelateById(context => context.Message.CorrelationId);
            });

            Initially(
                When(ReserveOfferEvent).
                Then(ctx => ctx.Saga.Registration = ctx.Message.Registration).
                Then(ctx => ctx.Saga.Offer = ctx.Message.Offer).
                Publish(ctx => new ReserveHotelEvent()
                {
                    CorrelationId = ctx.Saga.CorrelationId,
                    Reservation = new Models.Hotel.DTO.HotelReservationDTO()
                    {
                        HotelId = ctx.Saga.Offer.HotelId,
                        BeginDate = ctx.Saga.Offer.BeginDate,
                        EndDate = ctx.Saga.Offer.EndDate,
                        Rooms = ctx.Saga.Offer.Rooms
                    }
                }).
                TransitionTo(WaitingForHotel)
                );

            During(WaitingForHotel,
                When(ReserveHotelEvent).
                Then(ctx => processHotel(ctx.Message, ctx.Saga)).
                IfElse(ctx => ctx.Saga.MadeHotelReservation,
                valid => valid.Publish(ctx => new ReserveTransportEvent() { 
                    CorrelationId = ctx.Saga.CorrelationId,
                    Reservation = new Models.Transport.DTO.TransportReservationDTO()
                    {
                        ArrivalTransportId = ctx.Saga.Offer.Flight.DepartureTransportId,
                        ReturnTransportId = ctx.Saga.Offer.Flight.ReturnTransportId,
                        NumberOfPeople = ctx.Saga.Offer.NumberOfAdults + ctx.Saga.Offer.NumberOfNewborns + 
                        ctx.Saga.Offer.NumberOfTeenagers + ctx.Saga.Offer.NumberOfToddlers
                    }
                }).TransitionTo(ReservedOffer),
                invalid => invalid.Then(ctx => cancelReservations(ctx.Saga)).Publish(ctx => new ReserveOfferEventReply()
                {
                    Answer = ReserveOfferEventReply.State.NOT_RESERVED,
                    CorrelationId = ctx.Saga.CorrelationId,
                    Error = "Could not reserve hotel",
                    Registration = ctx.Saga.Registration
                }
                ).Finalize()));

            During(WaitingForTransport,
                When(ReserveTransportEvent).
                Then(ctx => processTransport(ctx.Message, ctx.Saga)).
                IfElse(ctx => ctx.Saga.MadeTransportReservation,
                valid => valid.Publish(ctx => new ReserveOfferEventReply()
                                {
                                    Answer = ReserveOfferEventReply.State.RESERVED,
                                    CorrelationId = ctx.Saga.CorrelationId,
                                    Registration = ctx.Saga.Registration
                                })
                                .Publish(ctx => new CheckPaymentEvent()
                                {
                                    CorrelationId = ctx.Saga.CorrelationId
                                })
                                .Schedule(PaymentNotSentTimeout, context => context.Init<PaymentTimeout>(new PaymentTimeout (){ CorrelationId = context.Saga.CorrelationId}))
                                .TransitionTo(ReservedOffer),
                invalid => invalid.Then(ctx => cancelReservations(ctx.Saga))
                                      .Publish(ctx => new ReserveOfferEventReply()
                                    {
                                        Answer = ReserveOfferEventReply.State.NOT_RESERVED,
                                        CorrelationId = ctx.Saga.CorrelationId,
                                        Error = "Could not reserve transport",
                                        Registration = ctx.Saga.Registration
                                    }).Finalize()));


            During(ReservedOffer,
                When(PaymentEvent).
                Finalize());



        }
    }
}
