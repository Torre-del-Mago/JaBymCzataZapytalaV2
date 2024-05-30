using HotelQuery.Consumer;
using HotelQuery.Repository.Hotel;
using HotelQuery.Repository.Reservation;
using HotelQuery.Service.Hotel;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// MongoDB context
builder.Services.AddScoped<IHotelRepository, HotelRepository>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRespository>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<CancelReservationHotelSyncConsumer>();
    cfg.AddConsumer<HotelQueryConsumer>();
    cfg.AddConsumer<HotelListQueryConsumer>();
    cfg.AddConsumer<ReserveHotelSyncConsumer>();

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
