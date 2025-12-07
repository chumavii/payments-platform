using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.Initiation.Api.Data;
using Payment.Initiation.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Payment Initiation Service API
builder.Services.AddDbContext<PaymentsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<PaymentsDbContext>(c =>
    {
        c.QueryTimeout = TimeSpan.FromSeconds(15);
        c.UsePostgres();
        c.UseBusOutbox();
        c.DisableInboxCleanupService();
    });

    x.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration["RabbitMQ:Host"], h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });
    });    
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
        var pendingMigrations = dbContext.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            dbContext.Database.Migrate();
        }
        else
        {
            Console.WriteLine("No pending migrations.");
        }
    }
}

app.UseHttpsRedirection();
app.MapPaymentInitiationEndpoints();
app.Run();

public partial class Program { }