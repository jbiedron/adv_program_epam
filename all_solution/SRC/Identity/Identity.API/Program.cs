using Identity.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
    .AddTestUsers(InMemoryConfig.GetUsers())
    .AddInMemoryClients(InMemoryConfig.GetClients())
    .AddInMemoryApiResources(InMemoryConfig.ApiResources)
    .AddInMemoryApiScopes(InMemoryConfig.ApiScopes)
    .AddDeveloperSigningCredential();                               

var app = builder.Build();

app.MapGet("/", () => "Hello Identity Manager!");

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseIdentityServer();

app.Run();
