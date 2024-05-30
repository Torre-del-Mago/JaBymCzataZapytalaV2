using MassTransit;
using TransportQuery.Consumer;
using TransportQuery.Repository.Transport;
using TransportQuery.Service.Transport;

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

builder.Services.AddMassTransit(cfg =>
{

    cfg.AddConsumer<TransportQueryConsumer>();
    cfg.AddConsumer<TransportListQueryConsumer>();
    cfg.AddConsumer<CancelReservationTransportSyncConsumer>();
    cfg.AddConsumer<TransportListQueryConsumer>();

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
