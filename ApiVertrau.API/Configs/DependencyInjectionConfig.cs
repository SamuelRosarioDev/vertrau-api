using System.Reflection;
using ApiVertrau.Application.Interfaces;
using ApiVertrau.Application.Services;
using ApiVertrau.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiVertrau.API.Configs;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services
            .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context
                        .ModelState.Where(e => e.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    var response = new
                    {
                        Status = 400,
                        Title = "Requisição Inválida",
                        Message = "Erro de validação nos dados enviados.",
                        Errors = errors,
                    };

                    return new BadRequestObjectResult(response);
                };
            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        services.AddScoped<IUsersServices, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
