namespace Models
{
    // Class to inherit after for differnt types of events
    public class EventModel
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public DateTime CreationDate { get; private set; }
        public EventModel(Guid id, DateTime date, Guid cId)
        {
            Id = id;
            CreationDate = date;
            CorrelationId = cId;
        }

        public EventModel()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
            CorrelationId = Guid.NewGuid();
        }
    }
}

// https://github.com/dotnet-architecture/eShopOnContainers/blob/main/src/BuildingBlocks/EventBus/EventBus/Events/IntegrationEvent.cs
// https://github.com/DistributedTravels/Models/blob/main/EventModel.cs
