using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NetconGeoAPI.Application.Services;
using NetconGeoAPI.Application.Validators;
using NetconGeoAPI.Domain.Context;
using NetconGeoAPI.Domain.Interfaces;
using NetconGeoAPI.Infrastructure.Repositories;
using NetconGeoAPI.Web.Endpoints;
using NetconGeoAPI.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("NetconDb"));
builder.Services.AddScoped<IFeasibilityRepository, FeasibilityRepository>();
builder.Services.AddScoped<FeasibilityService>();
// Registro do FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<FeasibilityRequestValidator>();


builder.Services.AddHealthChecks();
var app = builder.Build();
// 2. Popula o banco ao iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DataSeeder.Seed(db);
}
// Configure the HTTP request pipeline.
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<CustomHeaderResponseMiddleware>();

app.MapHealthChecks("/health");
app.MapFeasibilityEndpoints();
app.UseHttpsRedirection();

app.Run();
