using Identity.Server.Config.IdentityServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
    .AddInMemoryClients(InMemoryConfig.Clients)
    .AddInMemoryIdentityResources(InMemoryConfig.IdentityResources)
    .AddInMemoryApiResources(InMemoryConfig.ApiResources)
    .AddInMemoryApiScopes(InMemoryConfig.ApiScopes)
    .AddTestUsers(InMemoryConfig.TestUsers)
    .AddDeveloperSigningCredential();

var app = builder.Build();
app.UseRouting();
app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();
