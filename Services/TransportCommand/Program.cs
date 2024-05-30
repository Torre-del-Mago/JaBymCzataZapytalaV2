using MassTransit;
using Microsoft.EntityFrameworkCore;
using TransportCommand.Consumer;
using TransportCommand.Database;
using TransportCommand.Database.Tables;
using TransportCommand.Repository.EventRepository;
using TransportCommand.Repository.FlightConnectionRepository;
using TransportCommand.Repository.ReservedTicketRepository;
using TransportCommand.Repository.TransportRepository;
using TransportCommand.Service;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<TransportContext>(
    DbContextOptions => DbContextOptions.UseNpgsql(connectionString)
            .LogTo(Console.Write, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
    );
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TransportContext>();
builder.Services.AddScoped<IFlightConnectionRepository, FlightConnectionRepository>();
builder.Services.AddScoped<IReservedTicketRepository, ReservedTicketRepository>();
builder.Services.AddScoped<ITransportRepository, TransportRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<CancelReservationTransportConsumer>();
    cfg.AddConsumer<ReserveTransportConsumer>();
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
initDB();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

void initDB()
{
    using (var contScope = app.Services.CreateScope())
    using (var context = contScope.ServiceProvider.GetRequiredService<TransportContext>())
    {
        context.Database.EnsureCreated();
        if (!context.FlightConnections.Any())
        {
            string csvPath = @"InitData/transports.csv";
            using (var reader = new StreamReader(csvPath))
            {
                reader.ReadLine(); //Reading headers
                var random = new Random(123);
                int maxNumOfSeats = 20;
                int numberOfTransports = 90;
                double maxFlightPrice = 2500;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] elements = line.Split(',');
                    
                    FlightConnection flightConnection1 = new FlightConnection();
                    flightConnection1.DepartureLocation = elements[0];
                    flightConnection1.ArrivalCountry = elements[1];
                    flightConnection1.ArrivalLocation = elements[2];
                    
                    FlightConnection flightConnection2 = new FlightConnection();
                    flightConnection2.DepartureLocation = flightConnection1.ArrivalLocation;
                    flightConnection2.ArrivalCountry = "polska";
                    flightConnection2.ArrivalLocation = flightConnection1.DepartureLocation;
                    
                    var flightConnectionEntry1 = context.FlightConnections.Add(flightConnection1);
                    var flightConnectionEntry2 = context.FlightConnections.Add(flightConnection2);
                    
                    for (int days = 0; days < numberOfTransports; days++)
                    {
                        Transport transport1 = new Transport();
                        transport1.ConnectionId = flightConnectionEntry1.Entity.Id;
                        transport1.NumberOfSeats = random.Next(1, maxNumOfSeats);
                        transport1.DepartureDate = DateTime.SpecifyKind(DateTime.Now.AddDays(Convert.ToDouble(days)), DateTimeKind.Utc);
                        transport1.PricePerSeat = Convert.ToDecimal(Math.Round(random.NextDouble()*maxFlightPrice, 2));

                        Transport transport2 = new Transport();
                        transport2.ConnectionId = flightConnectionEntry2.Entity.Id;
                        transport2.NumberOfSeats = random.Next(1, maxNumOfSeats);
                        transport2.DepartureDate = DateTime.SpecifyKind(DateTime.Now.AddDays(Convert.ToDouble(days)), DateTimeKind.Utc);
                        transport2.PricePerSeat = Convert.ToDecimal(Math.Round(random.NextDouble()*maxFlightPrice, 2));

                        context.Transports.Add(transport1);
                        context.Transports.Add(transport2);
                    }
                }
            }
            context.SaveChanges();
        }
    }
}