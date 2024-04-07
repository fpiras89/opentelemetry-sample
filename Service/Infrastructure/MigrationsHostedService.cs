using Examples.Service.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Examples.Service.Infrastructure
{
    public class MigrationsHostedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public MigrationsHostedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
