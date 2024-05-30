using Models.Transport.DTO;

namespace TransportQuery.Service.Transport
{
    public interface ITransportService
    {
        TransportsDTO GetTransportsForCriteria(CriteriaForTransports criteria);
        TransportDTO GetTransportForCriteria(CriteriaForTransport criteria);
        Task<bool> ReserveTransport(TransportReservationDTO dto, int ArrivalTicketId, int ReturnTicketId);
        Task CancelTransport(int offerId);
    }
}
