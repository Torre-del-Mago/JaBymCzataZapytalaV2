using MassTransit;
using MongoDB.Driver;
using TransportQuery.Consumer;
using TransportQuery.Database.Entity;
using TransportQuery.Repository.Ticket;
using TransportQuery.Repository.Transport;
using TransportQuery.Service.Transport;
using static MassTransit.Util.ChartTable;

var builder = WebApplication.CreateBuilder(args);

// MongoDB context
builder.Services.AddScoped<ITransportRepository, TransportRepository>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITransportService, TransportService>();
builder.Services.AddScoped<ITransportRepository, TransportRepository>();
builder.Services.AddScoped<IReservedTicketRepository, ReservedTicketRepository>();

builder.Services.AddMassTransit(cfg =>
{

    cfg.AddConsumer<TransportQueryConsumer>();
    cfg.AddConsumer<TransportListQueryConsumer>();
    cfg.AddConsumer<CancelReservationTransportSyncConsumer>();
    cfg.AddConsumer<GetTopDepartureDestinationConsumer>();
    cfg.AddConsumer<ReserveTransportSyncConsumer>();
    cfg.AddConsumer<TransportListQueryConsumer>();
    cfg.AddConsumer<ChangeNumberOfSeatsSyncConsumer>();
    cfg.AddConsumer<ChangePricePerSeatSyncConsumer>();

    cfg.AddDelayedMessageScheduler();
    cfg.UsingRabbitMq((context, rabbitCfg) =>
    {
        rabbitCfg.Host(new Uri(builder.Configuration["MessageBroker:Host"]), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });
        rabbitCfg.UseDelayedMessageScheduler();
        rabbitCfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

initDB(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

void initDB(IServiceProvider services)
{
    using var scope = services.CreateScope();
    const string ConnectionString = "mongodb://root:example@mongo:27017/";
    var mongoClient = new MongoClient(ConnectionString);
    var database = mongoClient.GetDatabase("transport_query");

    var transportsCollection = database.GetCollection<Transport>("transports");
    var flightConnectionCollection = database.GetCollection<FlightConnection>("flight_connections");

    if (!transportsCollection.AsQueryable().Any())
    {
        string csvTransportPath = @"InitData/transports.csv";
        using (var reader = new StreamReader(csvTransportPath))
        {
            reader.ReadLine(); //Reading headers
            var random = new Random(123);
            int maxNumOfSeats = 20;
            int numberOfTransports = 90;
            double maxFlightPrice = 2500;
            string line;
            int connectionId1 = 1;
            int connectionId2 = 2;
            int IdTransport = 1;

            while ((line = reader.ReadLine()) != null)
            {
                string[] elements = line.Split(',');

                FlightConnection flightConnection1 = new FlightConnection();
                flightConnection1.DepartureLocation = elements[0];
                flightConnection1.ArrivalCountry = elements[1];
                flightConnection1.ArrivalLocation = elements[2];
                flightConnection1.Id = connectionId1;

                FlightConnection flightConnection2 = new FlightConnection();
                flightConnection2.DepartureLocation = flightConnection1.ArrivalLocation;
                flightConnection2.ArrivalCountry = "polska";
                flightConnection2.ArrivalLocation = flightConnection1.DepartureLocation;
                flightConnection2.Id = connectionId2;

                flightConnectionCollection.InsertOne(flightConnection1);
                flightConnectionCollection.InsertOne(flightConnection2);

                for (int days = 0; days < numberOfTransports; days++)
                {
                    Transport transport1 = new Transport();
                    transport1.ConnectionId = connectionId1;
                    transport1.NumberOfSeats = random.Next(1, maxNumOfSeats);
                    transport1.DepartureDate = DateOnly.FromDateTime(DateTime.SpecifyKind(DateTime.Now.AddDays(Convert.ToDouble(days)), DateTimeKind.Utc));
                    transport1.PricePerSeat = (float)Convert.ToDecimal(Math.Round(random.NextDouble() * maxFlightPrice, 2));
                    transport1.Id = IdTransport;
                    IdTransport++;

                    Transport transport2 = new Transport();
                    transport2.ConnectionId = connectionId2;
                    transport2.NumberOfSeats = random.Next(1, maxNumOfSeats);
                    transport2.DepartureDate = DateOnly.FromDateTime(DateTime.SpecifyKind(DateTime.Now.AddDays(Convert.ToDouble(days)), DateTimeKind.Utc));
                    transport2.PricePerSeat = (float)Convert.ToDecimal(Math.Round(random.NextDouble() * maxFlightPrice, 2));
                    transport2.Id = IdTransport;
                    IdTransport++;

                    transportsCollection.InsertOne(transport1);
                    transportsCollection.InsertOne(transport2);
                }
                connectionId1 += 2;
                connectionId2 += 2;
            }
        }
    }
}
