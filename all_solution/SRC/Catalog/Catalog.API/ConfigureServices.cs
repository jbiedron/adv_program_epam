using Infrastructure.Persistance;

namespace CatalogService.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddAPIServices(this IServiceCollection services)
        {
            // nothing to do for now...
            services.AddHealthChecks();

            // #region Assembly Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore, Version=6.0.5.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
            // C:\Users\Jaroslaw_Biedron\.nuget\packages\microsoft.extensions.diagnostics.healthchecks.entityframeworkcore\6.0.5\lib\net6.0\Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore.dll
            // .AddDbContextCheck<ApplicationDbContext>();

            return services;
        }
    }
}
