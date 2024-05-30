using Models.Transport.DTO;

namespace TransportQuery.Service.Transport
{
    public interface ITransportService
    {
        public TransportsDTO GetTransportsForCriteria(CriteriaForTransports criteria);
        public TransportDTO GetTransportForCriteria(CriteriaForTransport criteria);

        public Task<bool> ReserveTransport(TransportReservationDTO dto);

        public Task CancelTransport(int offerId);
    }
}
