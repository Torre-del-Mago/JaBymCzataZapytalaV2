using Models.Transport.DTO;

namespace TransportQuery.Service.Transport
{
    public interface ITransportService
    {
        public TransportDTO getTransportForCriteria(CriteriaForTransport criteria);
    }
}
