using MassTransit;
using OfferCommand.Consumer;
using OfferCommand.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OfferContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//builder.Services.AddMassTransit(cfg =>
//{
//    // adding consumers
//    cfg.AddConsumer<ReserveOfferConsumer>();

//    // telling masstransit to use rabbitmq
//    cfg.UsingRabbitMq((context, rabbitCfg) =>
//    {
//        // rabbitmq config
//        rabbitCfg.Host("rabbitmq", "/", h =>
//        {
//            h.Username("guest");
//            h.Password("guest");
//        });
//        // automatic endpoint configuration (and I think the reason why naming convention is important
//        rabbitCfg.ConfigureEndpoints(context);
//    });
//});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
