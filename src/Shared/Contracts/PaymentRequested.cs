namespace Contracts;

public record PaymentRequested(
    Guid PaymentId,
    Guid CustomerId,
    long AmountCents,
    string Currency
);
