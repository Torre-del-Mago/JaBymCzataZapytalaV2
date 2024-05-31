using MassTransit;
using Microsoft.EntityFrameworkCore;
using OfferCommand;
using OfferCommand.Consumer;
using OfferCommand.Database;
using OfferCommand.Repository.EventRepository;
using OfferCommand.Repository.OfferRepository;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<OfferContext>(
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
builder.Services.AddDbContext<OfferContext>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IOfferRepository, OfferRepository>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddSagaStateMachine<OfferSaga, OfferReservation>(context =>
    {
        context.UseInMemoryOutbox();
    }).InMemoryRepository();
    cfg.AddConsumer<PaidOfferConsumer>();
    cfg.AddConsumer<RemoveOfferConsumer>();
    cfg.AddConsumer<ReserveOfferConsumer>();
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
using (var contScope = app.Services.CreateScope())
using (var context = contScope.ServiceProvider.GetRequiredService<OfferContext>())
{
    context.Database.EnsureCreated();
    context.SaveChanges();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
