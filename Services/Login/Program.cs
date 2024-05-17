using Login.Consumer;
using MassTransit;
using Models.Login;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddMassTransit(conf =>
{
    conf.SetKebabCaseEndpointNameFormatter();
    conf.SetInMemorySagaRepositoryProvider();
    var asb = typeof(Program).Assembly;

    conf.AddConsumers(asb);
    conf.AddSagaStateMachines(asb);
    conf.AddActivities(asb);

    conf.AddConsumer<LoginConsumer>();

    conf.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("custom-queue-name", x =>
        {
            x.ConfigureConsumer<LoginConsumer>(ctx);
        });
    });
});
//builder.Services.AddMassTransit(cfg =>
//{


//    // adding consumers
//    cfg.AddConsumer<LoginConsumer>();

//    // telling masstransit to use rabbitmq
//    cfg.UsingRabbitMq((context, rabbitCfg) =>
//    {
//        // rabbitmq config
//        rabbitCfg.Host("rabbitmq://localhost", "/", h =>
//        {
//            h.Username("guest");
//            h.Password("guest");
//        });

//        rabbitCfg.ReceiveEndpoint("custom-queue-name", x =>
//        {
//            //x.Bind("exchange-name");
//            //x.Bind<CheckLoginEvent>();
//            x.ConfigureConsumer<LoginConsumer>(context);        
//        });

//        rabbitCfg.ConfigureEndpoints(context);
//    });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();