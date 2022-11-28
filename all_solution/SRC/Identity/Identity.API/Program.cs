using Identity.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
    .AddTestUsers(InMemoryConfig.GetUsers())
    .AddInMemoryClients(InMemoryConfig.GetClients())
    .AddDeveloperSigningCredential();                               

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseIdentityServer();

app.Run();
