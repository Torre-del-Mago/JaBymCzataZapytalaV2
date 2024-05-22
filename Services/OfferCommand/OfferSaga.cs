using MassTransit;
using Models.Offer;
using Models.Offer.DTO;
using Models.Payment;

namespace OfferCommand
{
    public class OfferReservation : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }

        public int Registration { get; set; }

        public OfferDTO Offer { get; set; }

        public bool MadeReservation {  get; set; }
        public bool PaidForReservation {  get; set; }

    }
    public class OfferSaga: MassTransitStateMachine<OfferReservation>
    {
        public State ReservedOffer {  get; set; }
        public State PaidForOffer { get; set; }

        public Event<ReserveOfferEvent> reserveOfferEvent { get; set; }
        public Event<CheckPaymentEventReply> checkPaymentEvent { get; set; }

        private void processReservation()
        {

        }

        private void processPayment()
        {

        }

        private void cancelReservations()
        {

        }

        public OfferSaga()
        {
            InstanceState(x => x.CurrentState);

            Initially(
                When(reserveOfferEvent).
                Then(ctx => ctx.Saga.Registration = ctx.Message.Registration).
                Then(ctx => ctx.Saga.Offer = ctx.Message.Offer).
                Then(ctx => processReservation()).
                IfElse(ctx => ctx.Saga.MadeReservation,
                valid => valid.TransitionTo(ReservedOffer),
                invalid => invalid.Then(ctx => cancelReservations()).Finalize())
                );

            During(ReservedOffer,
                When(checkPaymentEvent).
                Then(ctx => processPayment()).
                IfElse(ctx => ctx.Saga.PaidForReservation,
                valid => valid.Finalize(),
                invalid => invalid.Then(ctx => cancelReservations()).Finalize()));



        }
    }
}
