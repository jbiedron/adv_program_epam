using Carting.API.Extensions;
using Carting.API.Messaging;
using Carting.API.Messaging.Service;
using CartingService.DAL.Repository;
using CartingService.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersioningConfigured();

/*
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);

    // Set a default version when it's not provided,
    // e.g., for backward compatibility when applying versioning on existing APIs
    options.AssumeDefaultVersionWhenUnspecified = true;

    // ReportApiVersions will return the "api-supported-versions" and "api-deprecated-versions" headers.
    options.ReportApiVersions = true;
});*/

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Protected API", Version = "v1" });
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://localhost:5000/connect/authorize"),
                TokenUrl = new Uri("https://localhost:5000/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    {"Catalog.API", "Catalog.API"}
                }
            }
        }
    });
});


/*
// redis
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "CartingService_";
});
*/

builder.Services.AddSingleton<ConnectionMultiplexer>(sp =>
{
    // var settings = sp.GetRequiredService<IOptions<BasketSettings>>().Value;
    // var configuration = ConfigurationOptions.Parse(settings.ConnectionString, true);
    var configuration = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddTransient<ICartingRespository, RedisCartingRespository>();
builder.Services.AddTransient<CartService>();

// bg service for event receiver
var serviceClientSettingsConfig = builder.Configuration.GetSection("RabbitMq");
builder.Services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);
builder.Services.AddHostedService<ProductUpdateReceiver>();

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
