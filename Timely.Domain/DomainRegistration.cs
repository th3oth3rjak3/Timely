namespace Timely.Domain;
using Microsoft.Extensions.DependencyInjection;
using Timely.Domain.Features.GlobalTimer;

public static class DomainRegistration
{
    public static IServiceCollection RegisterDomainServices(this IServiceCollection services)
    {
        _ = services.AddSingleton<IGlobalTimerService, GlobalTimerService>();
        return services;
    }
}
