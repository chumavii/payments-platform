using Contracts;
using MassTransit;
using Payment.Initiation.Api.Data;
using Payment.Initiation.Api.Domain;

namespace Payment.Initiation.Api.Endpoints
{
    public static class PaymentInitiationEndpoints
    {
        public static IEndpointRouteBuilder MapPaymentInitiationEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/payments/initiate");

            group.MapPost("/", async (
                InitiatePaymentDto requestDto,
                PaymentsDbContext db,
                IPublishEndpoint publish,
                CancellationToken ct) =>
            {
                var payment = new InitiatePayment
                {
                    Id = Guid.NewGuid(),
                    CustomerId = requestDto.CustomerId,
                    Amount = requestDto.Amount,
                    Currency = requestDto.Currency
                };

                await db.Payments.AddAsync(payment);

                var evt = new PaymentRequested(payment.Id, payment.CustomerId, payment.Amount, payment.Currency);
                await publish.Publish(evt, ct);

                await db.SaveChangesAsync(ct);

                return Results.Created($"/payments/{payment.Id}", payment);
            });

            return app;
        }
    }
}

// DTO for API input
public record InitiatePaymentDto(Guid CustomerId, decimal Amount, string Currency);
