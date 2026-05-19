using CapaServicio.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API_TRANSPORTISTE.Services
{
    public class TareaProgramadaService : BackgroundService
    {
        private readonly ILogger<TareaProgramadaService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public TareaProgramadaService(
            ILogger<TareaProgramadaService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de tarea programada iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await EjecutarTareaProgramada();

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Servicio cancelado.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en la tarea programada.");

                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }

            _logger.LogInformation("Servicio detenido.");
        }

        private async Task EjecutarTareaProgramada()
        {
            _logger.LogInformation(
                $"Ejecutando tarea programada - {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            try
            {
                using var scope = _scopeFactory.CreateScope();

                var service =
                    scope.ServiceProvider.GetRequiredService<ITransportistaService>();

                await service.LimpiarBloqueoAsientos();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Ocurrió un error al limpiar el bloqueo de asientos.");
            }

            _logger.LogInformation("Tarea programada completada.");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deteniendo servicio.");

            await base.StopAsync(cancellationToken);
        }
    }
}