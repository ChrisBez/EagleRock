using EagleRock.Business;
using EagleRock.Cache;
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

builder.Services.AddTransient<ICacheService, CacheService>();
builder.Services.AddTransient<IEagleService, EagleService>();

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
