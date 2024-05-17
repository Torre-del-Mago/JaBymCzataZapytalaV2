using MassTransit;
using MassTransit.RabbitMqTransport;
using Models.Login;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddMassTransit(cfg =>
//{
//    cfg.SetKebabCaseEndpointNameFormatter();

//    cfg.UsingRabbitMq((context, rabbitCfg) =>
//    {

//        rabbitCfg.Host(new Uri(builder.Configuration["MessageBroker:Host"]), h =>
//        {
//            h.Username(builder.Configuration["MessageBroker:Username"]);
//            h.Password(builder.Configuration["MessageBroker:Password"]);
//        });

//        rabbitCfg.ConfigureEndpoints(context);
//    });
//});
builder.Services.AddMassTransit(cfg =>
{
    cfg.AddDelayedMessageScheduler();
    cfg.UsingRabbitMq((context, rabbitCfg) =>
    {
        rabbitCfg.Host(new Uri(builder.Configuration["MessageBroker:Host"]), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        rabbitCfg.UseDelayedMessageScheduler();
        rabbitCfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "app v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
