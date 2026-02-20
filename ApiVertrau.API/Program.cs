using ApiVertrau.API.Configs;
using ApiVertrau.API.Middlewares;
using ApiVertrau.Infrastructure.TypeHandlers;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

SqlMapper.AddTypeHandler(new SqliteDateOnlyHandler<DateOnly>());
SqlMapper.AddTypeHandler(new SqliteDateOnlyHandler<DateOnly?>());

builder.Services.AddApiConfiguration();
builder.Services.AddDbConfig(builder.Configuration);

var app = builder.Build();

app.UseMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
