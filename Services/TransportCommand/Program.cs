using MassTransit;
using TransportCommand.Consumer;
using TransportCommand.Database;
using TransportCommand.Repository.EventRepository;
using TransportCommand.Repository.FlightConnectionRepository;
using TransportCommand.Repository.ReservedTicketRepository;
using TransportCommand.Repository.TransportRepository;
using TransportCommand.Service;

var builder = WebApplication.CreateBuilder(args);

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
