using Contracts;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Worker starting...");

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<PaymentRequestedConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(context.Configuration["RabbitMQ:Host"], "/", h =>
                {
                    h.Username(context.Configuration["RabbitMQ:Username"] ?? "guest");
                    h.Password(context.Configuration["RabbitMQ:Password"] ?? "guest");
                });

                cfg.ReceiveEndpoint("payment_adapter_queue", e =>
                {
                    e.ConfigureConsumer<PaymentRequestedConsumer>(ctx);
                });
            });
        });
    });