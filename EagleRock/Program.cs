using EagleRock.Business;
using EagleRock.Cache;
using EagleRock.Publisher;
using MassTransit;
using Microsoft.FeatureManagement;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "EagleRock.xml"));
});

//Redis DI as per googling
var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration["RedisAddress"]);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);


builder.Services.AddFeatureManagement();

builder.Services.AddMassTransit(m =>
{
    m.UsingRabbitMq((r, c) =>
    {
        c.Host(builder.Configuration["RabbitMqAddress"]);
    });
});

builder.Services.AddMassTransitHostedService();


builder.Services.AddTransient<ICacheService, CacheService>();
builder.Services.AddTransient<IEagleService, EagleService>();
builder.Services.AddTransient<IMessagingService, MessagingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//TODO: Implement the asp.net core HealthCheck

app.UseAuthorization();

app.MapControllers();

app.Run();
