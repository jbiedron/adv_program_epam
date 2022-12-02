using Identity.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer(options => {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
    })
    .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
    .AddTestUsers(InMemoryConfig.GetUsers())
    .AddInMemoryClients(InMemoryConfig.GetClients())
    .AddInMemoryApiResources(InMemoryConfig.ApiResources)
    .AddInMemoryApiScopes(InMemoryConfig.ApiScopes)
    .AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication();

var app = builder.Build();

// app.MapGet("/", () => "Hello Identity Manager!");

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});


app.Run();
