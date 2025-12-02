using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Payment.Initiation.Api.Data;
using Testcontainers.PostgreSql;

namespace Payment.Initiation.Tests
{
    public class TestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer =
            new PostgreSqlBuilder()
                .WithUsername("postgres")
                .WithPassword("postgres")
                .WithDatabase("payment_test")
                .Build();

        public string ConnectionString => _dbContainer.GetConnectionString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove real db
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<PaymentsDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                // Add test db
                services.AddDbContext<PaymentsDbContext>(options =>
                    options.UseNpgsql(ConnectionString));

                // Remove MassTransit 
                var massTransitDescriptor = services.FirstOrDefault(d =>
                    d.ServiceType == typeof(IBusControl));
                if (massTransitDescriptor != null)
                    services.Remove(massTransitDescriptor);

                // Add MassTransit test harness
                services.AddMassTransitTestHarness(cfg =>
                {
                    cfg.AddEntityFrameworkOutbox<PaymentsDbContext>(c =>
                    {
                        c.QueryTimeout = TimeSpan.FromSeconds(15);
                        c.UsePostgres();
                        c.UseBusOutbox();
                    });
                });

                // Override IPublishEndpoint to use the test harness bus
                services.AddScoped<IPublishEndpoint>(sp =>
                    sp.GetRequiredService<ITestHarness>().Bus);
            });
        }
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            // Run migrations
            using (var scope = Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
                await db.Database.MigrateAsync();
            }
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }
    }
}
