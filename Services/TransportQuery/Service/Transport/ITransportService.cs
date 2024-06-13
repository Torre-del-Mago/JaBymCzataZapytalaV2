using Models.Admin.DTO;
using Models.Transport.DTO;

namespace TransportQuery.Service.Transport
{
    public interface ITransportService
    {
        TransportsDTO GetTransportsForCriteria(CriteriaForTransports criteria);
        TransportDTO GetTransportForCriteria(CriteriaForTransport criteria);
        Task<bool> ReserveTransport(TransportReservationDTO dto, int ArrivalTicketId, int ReturnTicketId);
        Task CancelTransport(int offerId);
        TopDepartureDTO GetTopDepartures(int numberOfElements);
        TopDestinationDTO GetTopDestinations(int numberOfElements);
        void ChangeNumberOfSeats(int transportId, int numberOfSeats);
        void ChangePricePerSeat(int transportId, double priceChange);
    }
}
