using FluentValidation;
using Microsoft.OpenApi.Models;
using RotaViagem.application.Interfaces;
using RotaViagem.application.Mappers;
using RotaViagem.application.Services;
using RotaViagem.application.Utils;
using RotaViagem.application.Validations;
using RotaViagem.domain.Entities;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDataService<Rota>>(
    sp => new JsonDataService<Rota>("Data/rotas.json"));

builder.Services.AddScoped<IRotaService, RotaService>();
builder.Services.AddScoped<IValidator<Rota>, RotaValidation>();
builder.Services.AddScoped<IRotaMapper, RotaMapper>();

builder.Services.AddScoped<IDijkstraService>(sp =>
{
    // Obtenha ou crie o grafo aqui
    var grafo = new Dictionary<string, List<(string destino, double custo)>>();
    // Inicialize o grafo conforme necessário

    return new DijkstraService(grafo);
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Simulador de Rotas de Voo", Version = "v1" });
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
