namespace Timely.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Timely.Domain.Features.TodoItems;
using Timely.Persistence.Repositories;

public static class PersistenceRegistration
{
    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, string appDataDirectory)
    {
        var path = Path.Combine(appDataDirectory, "timely.db3");

        services.AddTransient<TimelyContext>(s => new TimelyContext(path));

        services.AddTransient<ITodoItemRepository, TodoItemRepository>();

        var dbContext = new TimelyContext(path);
        SQLitePCL.Batteries_V2.Init();
        dbContext.Database.Migrate();
        dbContext.Dispose();

        return services;
    }
}
