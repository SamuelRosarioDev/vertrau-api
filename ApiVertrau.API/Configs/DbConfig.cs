using ApiVertrau.Infrastructure.Migrations;
using FluentMigrator.Runner;

namespace ApiVertrau.API.Configs;

public static class DbConfig
{
    public static IServiceCollection AddDbConfig(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb =>
                rb.AddSQLite()
                    .WithGlobalConnectionString(
                        configuration.GetConnectionString("DefaultConnection")
                    )
                    .ScanIn(typeof(CreateUsersTable).Assembly)
                    .For.Migrations()
            )
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        return services;
    }

    public static void UseMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}
