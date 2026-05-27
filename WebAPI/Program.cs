using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using WebAPI.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDomain();
builder.Services.AddWebApi(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TransactManager API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("TransactManagerCors");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<TransactManagerContext>>();
    try
    {
        logger.LogInformation("Verificando e aplicando Migrations pendentes no banco de dados");
        var context = services.GetRequiredService<TransactManagerContext>();
        context.Database.Migrate();
        logger.LogInformation("Migrations aplicadas com sucesso");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ocorreu um erro ao tentar aplicar as migrations.");
    }
}

app.Run();
