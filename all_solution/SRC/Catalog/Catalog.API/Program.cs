using Application.Extensions.DependencyInjection;
using CatalogService.Extensions.DependencyInjection;
using Infrastructure.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Catalog.Messaging.Send.Options;
using Catalog.Messaging.Send.Sender;
using Microsoft.IdentityModel.Tokens;
using Catalog.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

  //  options.SwaggerDoc("v1", new OpenApiInfo { Title = "Protected API", Version = "v1" });
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://localhost:7215/connect/authorize"),
                TokenUrl = new Uri("https://localhost:7215/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    //  { IdentityServerConstants.StandardScopes.OpenId, "Catalog.API", "roles" 
                    {"Catalog.API", "Catalog.API"},
                    {"openid", "openid"},
                    {"roles", "roles"}
                }
            }
        }
    });
    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddAPIServices();

// rabbitmq - TODO: move it to extensions method!
var serviceClientSettingsConfig = builder.Configuration.GetSection("RabbitMq");
builder.Services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);
builder.Services.AddTransient<IProductUpdateSender, ProductUpdateSender>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", opt =>
    {
        opt.RequireHttpsMetadata = false;
        opt.Authority = "https://localhost:7215";       // Identity.Server
        opt.Audience = "Catalog.API";                   // this is api scope
    });

builder.Services.AddAuthorization();
/*
builder.Services.AddAuthorization(opt => {
    // policies provides more flexibility than only roles
    opt.AddPolicy("BuyerRead", policyBuilder =>
    {
       // policyBuilder.RequireAuthenticatedUser();
     //   policyBuilder.RequireClaim("role", "Buyer");
        // we can add more claims based on 
    });
    opt.AddPolicy("ManagerFull", policyBuilder => {
   //     policyBuilder.RequireClaim("role", "Manager");
        // we can add more claims based on 
    });
});
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

        options.OAuthClientId("client_api_swagger");
        options.OAuthAppName("Catalog API - Swagger");
        options.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
