using Models.Transport.DTO;

namespace TransportQuery.Service.Transport
{
    public interface ITransportService
    {
        TransportsDTO GetTransportsForCriteria(CriteriaForTransports criteria);
        TransportDTO GetTransportForCriteria(CriteriaForTransport criteria);
        Task<bool> ReserveTransport(TransportReservationDTO dto);
        Task CancelTransport(int offerId);
    }
}
