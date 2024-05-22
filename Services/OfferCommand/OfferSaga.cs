using MassTransit;
using Models.Hotel;
using Models.Offer;
using Models.Offer.DTO;
using Models.Payment;
using Models.Transport;

namespace OfferCommand
{
    public class OfferReservation : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }

        public int Registration { get; set; }

        public OfferDTO Offer { get; set; }

        public bool MadeReservation {  get; set; }
        public bool MadeHotelReservation {  get; set; }
        public bool MadeTransportReservation {  get; set; }
        public bool PaidForReservation {  get; set; }

    }
    public class OfferSaga: MassTransitStateMachine<OfferReservation>
    {
        public State WaitingForHotel {  get; set; }
        public State WaitingForTransport { get; set; }
        public State ReservedOffer {  get; set; }

        public Event<ReserveOfferEvent> ReserveOfferEvent { get; set; }
        public Event<CheckPaymentEventReply> PaymentEvent { get; set; }
        public Event<ReserveHotelEventReply> ReserveHotelEvent { get; set; }
        public Event<ReserveTransportEventReply> ReserveTransportEvent { get; set; }

        private void sendHotelReservation()
        {

        }

        private void sendTransportReservation()
        {

        }

        private void startPayment()
        {

        }

        private void processPayment()
        {

        }

        private void cancelReservations()
        {

        }

        private void processHotel()
        {

        }

        private void processTransport()
        {

        }

        private void sendPaymentMessage()
        {

        }

        private void publishReserveOfferResponse()
        {

        }

        private void publishPaymentResponse()
        {

        }

        public OfferSaga() 
        {
            InstanceState(x => x.CurrentState);

            Initially(
                When(ReserveOfferEvent).
                Then(ctx => ctx.Saga.Registration = ctx.Message.Registration).
                Then(ctx => ctx.Saga.Offer = ctx.Message.Offer).
                Then(ctx => sendHotelReservation()).
                TransitionTo(WaitingForHotel)
                );

            During(WaitingForHotel,
                When(ReserveHotelEvent).
                Then(ctx => processHotel()).
                IfElse(ctx => ctx.Saga.MadeHotelReservation,
valid => valid.Then(ctx => sendTransportReservation()).TransitionTo(ReservedOffer),
invalid => invalid.Then(ctx => cancelReservations()).Finalize()));

            During(WaitingForTransport,
                When(ReserveTransportEvent).
                Then(ctx => processTransport()).
                IfElse(ctx => ctx.Saga.MadeTransportReservation,
valid => valid.Then(ctx => publishReserveOfferResponse()).Then(ctx => sendPaymentMessage()).TransitionTo(ReservedOffer),
invalid => invalid.Then(ctx => cancelReservations()).Finalize()));


            During(ReservedOffer,
                When(PaymentEvent).
                Then(ctx => processPayment()).
                IfElse(ctx => ctx.Saga.PaidForReservation,
                valid => valid.Then(ctx => publishPaymentResponse()).Finalize(),
                invalid => invalid.Then(ctx => cancelReservations()).Finalize()));



        }
    }
}
