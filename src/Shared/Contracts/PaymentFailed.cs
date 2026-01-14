using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class PaymentFailed
    {
        public Guid PaymentId { get; init; }
        public Guid CustomerId { get; init; }
        public long Amount { get; init; }
        public required string Currency { get; init; }
        public required string Reason { get; init; }
        public DateTime FailedAt { get; init; }
    }
}
