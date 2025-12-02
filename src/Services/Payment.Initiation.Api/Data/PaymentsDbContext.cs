using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Payment.Initiation.Api.Domain;

namespace Payment.Initiation.Api.Data
{
    public class PaymentsDbContext : DbContext
    {
        public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options) 
        {
        }
        public DbSet<InitiatePayment> Payments { get; set; } = null!;
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
        public DbSet<OutboxState> OutboxStates => Set<OutboxState>();
        public DbSet<InboxState> InboxStates => Set<InboxState>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InitiatePayment>()
                .ToTable("payment_requests")
                .HasKey(x => x.Id);

            modelBuilder.AddOutboxMessageEntity();

            modelBuilder.AddOutboxStateEntity();

            modelBuilder.AddInboxStateEntity();
        }
    }
}
