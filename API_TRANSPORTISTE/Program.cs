using CapaDatos.Interfaces;
using CapaDatos.Repositorio;
using CapaServicio.Interfaces;
using CapaServicio.Servicios;
using API_TRANSPORTISTE.Authentication;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Repositories
builder.Services.AddScoped<ITransportistaRepository, TransportistaRepository>();

// Services
builder.Services.AddScoped<ITransportistaService, TransportistaService>();

// Configurar autenticación por API Key (simplificado)
builder.Services.AddAuthentication("ApiKey")
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);

builder.Services.AddAuthorization();

// OpenAPI
builder.Services.AddOpenApi();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    // En producción, forzar HTTPS y agregar HSTS
    app.UseHsts();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

// Importante: El orden es Authentication → Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
