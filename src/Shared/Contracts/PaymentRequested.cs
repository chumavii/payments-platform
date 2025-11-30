namespace Contracts;

public record PaymentRequested(
    Guid PaymentId,
    Guid CustomerId,
    decimal Amount,
    string Currency
);
