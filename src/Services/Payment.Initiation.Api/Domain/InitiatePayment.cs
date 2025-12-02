using System.ComponentModel.DataAnnotations;

namespace Payment.Initiation.Api.Domain
{
    public class InitiatePayment
    {
        public Guid Id { get; set; }
        [Required]
        public required Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "CAD";
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
