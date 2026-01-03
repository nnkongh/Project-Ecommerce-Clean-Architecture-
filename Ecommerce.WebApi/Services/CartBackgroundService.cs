
using Ecommerce.Application.Interfaces;

namespace Ecommerce.WebApi.Services
{
    public class CartBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly TimeSpan _time = TimeSpan.FromMinutes(1);
        private readonly ILogger<CartBackgroundService> _logger;
        public CartBackgroundService(IServiceProvider provider, ILogger<CartBackgroundService> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background service start");
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _provider.CreateScope())
                {
                    var cartServicce = scope.ServiceProvider.GetRequiredService<ICartExpirationService>();
                    await cartServicce.HandleExpiredCartAsync(stoppingToken);
                }
                await Task.Delay(_time,stoppingToken);
            }
        }
    }
}
