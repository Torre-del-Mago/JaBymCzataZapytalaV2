using HotelCommand.Consumer;
using HotelCommand.Database;
using HotelCommand.Repository.DietRepository;
using HotelCommand.Repository.HotelRepository;
using HotelCommand.Repository.HotelDietRepository;
using HotelCommand.Repository.HotelRoomTypeRepository;
using HotelQuery.Consumer;
using MassTransit;
using HotelCommand.Repository.ReservationRepository;
using HotelCommand.Repository.ReservedRoomRepository;
using HotelCommand.Repository.RoomTypeRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HotelContext>();
builder.Services.AddScoped<IDietRepository, DietRepository>();
builder.Services.AddScoped<IHotelDietRepository, HotelDietRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelRoomTypeRepository, HotelRoomTypeRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservedRoomRepository, ReservedRoomRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<CancelReservationHotelConsumer>();
    cfg.AddConsumer<ReserveHotelConsumer>();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
