using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public class RoleSeederHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvier;

        public RoleSeederHostedService(IServiceProvider serviceProvider) {
            _serviceProvier = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scoped = _serviceProvier.CreateScope();
            await SeedRole.SeedRoleAsync(scoped.ServiceProvider);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
