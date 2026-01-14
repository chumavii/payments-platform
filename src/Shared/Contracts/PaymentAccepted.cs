using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public record PaymentAccepted
    {
        public Guid PaymentId { get; init; }
        public Guid CustomerId { get; init; }
        public long AmountCents { get; init; }
        public required string Currency { get; init; }
        public required string GatewayReference { get; init; }
        public DateTime AcceptedAt { get; init; }
    }
}
