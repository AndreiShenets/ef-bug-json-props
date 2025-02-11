using EfCoreException;
using Microsoft.EntityFrameworkCore;
using static System.Console;

async Task ReproduceBugAsync(IHost host)
{
    using IServiceScope serviceScope = host.Services.CreateScope();
    DatabaseContext context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();

    Guid entityId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    Entity entity = context.Entities.First(e => e.Id == entityId);

    entity.L1OwnedEntities.First().L2OwnedEntities.Add(
        new OwnedEntityLevel2
        {
            Values =
            [
                new EntityValue
                {
                    Name = "Name3",
                    Value = "Value3"
                },
                new EntityValue
                {
                    Name = "Name4",
                    Value = "Value4"
                }
            ]
        }
    );

    await context.SaveChangesAsync();
}

try
{
    HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddDbContextFactory(builder.Configuration);

    IHost host = builder.Build();

    await host.StartAsync();

    await host.RecreateDatabaseAsync();

    await ReproduceBugAsync(host);

    await host.StopAsync();
}
catch (Exception e)
{
    WriteLine(e);
}

WriteLine("Done");


internal static partial class Program
{
    private static async Task RecreateDatabaseAsync(this IHost app)
    {
        using IServiceScope serviceScope = app.Services.CreateScope();
        DatabaseContext context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();
        await context.SeedDatabaseAsync();
    }

    private static async Task SeedDatabaseAsync(this DatabaseContext context)
    {
        Guid entityId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        if (await context.Entities.AnyAsync(e => e.Id == entityId))
        {
            return;
        }

        context.Entities.Add(
            new Entity
            {
                Id = entityId,
                L1OwnedEntities =
                [
                    new OwnedEntityLevel1
                    {

                        L2OwnedEntities =
                        [
                            new OwnedEntityLevel2
                            {
                                Values =
                                [
                                    new EntityValue
                                    {
                                        Name = "Name1",
                                        Value = "Value1"
                                    },
                                    new EntityValue
                                    {
                                        Name = "Name2",
                                        Value = "Value2"
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }
        );

        await context.SaveChangesAsync();
    }

    private static void AddDbContextFactory(this IServiceCollection services, IConfigurationRoot configurationRoot)
    {
        const string ConnectionStringName = "SqlServer";

        string connectionString = configurationRoot.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' must be provided");

        services.AddDbContextFactory<DatabaseContext>(
            opts =>
            {
                opts.UseSqlServer(
                    connectionString,
                    options =>
                    {
                        options.EnableRetryOnFailure();
                    }
                );
            }
        );
    }
}
