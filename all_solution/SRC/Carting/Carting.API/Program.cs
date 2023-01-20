using Carting.API.Extensions;
using Carting.API.Messaging;
using Carting.API.Messaging.Service;
using Carting.API.Options;
using CartingService.DAL.Repository;
using CartingService.Service;
using EventBusRabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Reflection;

/*
using EventBusRabbitMQ.OLD.RabbitMQ.v2;
using EventBusRabbitMQ.OLD.Core.v2;
using EventBusRabbitMQ.OLD.Catalog.v2;
*/

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(o =>
{
    o.ValidateOnBuild = true;
    o.ValidateScopes = true;
});

/*
var builder = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => { })
    .UseDefaultServiceProvider((context, options) => {
        options.ValidateOnBuild = true;
    });
*/

// Add services to the container.

builder.Services.AddControllers();

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
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();


/*
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
*/
builder.Services.AddApiVersioningConfigured();


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

/*  TODO: check wy config does not load properly? empty!
var serviceClientSettingsConfig = builder.Configuration.GetSection("RabbitMq");
builder.Services.Configure<EventBusRabbitMQ.RabbitMQ.RabbitMqConfiguration>(serviceClientSettingsConfig);
builder.Services.AddHostedService<ProductUpdateReceiver>();

*/
/*
// builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();
builder.Services.AddSingleton<IEventBus, RabbitMQEventBusv2>();
builder.Services.AddSingleton<IEventBusPersistentConnection, RabbitMQPersistentConnection>();

void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    eventBus.Subscribe<ProductPriceChangedIntegrationEvent>();
}
*/
/*
eventBus.Subscribe<ProductPriceChangedIntegrationEvent, ProductPriceChangedIntegrationEventHandler>();
eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();
8/

}

// event bus - v2

/*
var serviceClientSettingsConfig = builder.Configuration.GetSection("RabbitMq");
builder.Services.Configure<EventBusRabbitMQ.RabbitMQ.RabbitMqConfiguration>(serviceClientSettingsConfig);
*/

// service bus - v2
// builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();
/*
builder.Services.AddSingleton<IEventBus, RabbitMQEventBusv2>();

builder.Services.AddSingleton<IEventBusPersistentConnection, RabbitMQPersistentConnection>();
*/

// builder.Services.AddSingleton<IProductPriceUpdateSender, ProductPriceUpdateSender>();

// event bus - LAST VERSION !!!!

builder.Services.AddTransient<ICommandSubscriber, ProductPriceUpdatedHandler>();

var app = builder.Build();
// this is dirty way to initialize singleton and add event handlers on the client side
app.Services.GetService<ICommandSubscriber>();

app.UseSwagger();
app.UseSwaggerUI(options => {
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.ApiVersion.ToString());
    }
});

/*
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ads subscribers to event
// ConfigureEventBus(app);

app.Run();

