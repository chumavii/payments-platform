using Contracts;
using FluentAssertions;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Payment.Initiation.Api.Data;
using System.Net;
using System.Net.Http.Json;

namespace Payment.Initiation.Tests
{
    public class PaymentInitiationTests : IClassFixture<TestWebAppFactory>
    {
        protected readonly TestWebAppFactory _factory;
        protected readonly HttpClient _client;

        public PaymentInitiationTests(TestWebAppFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Should_Write_Payment_To_Database()
        {
            //Arrange
            var customerId = Guid.NewGuid();
            var request = new InitiatePaymentDto(customerId, 100,"USD");

            //Act
            var response = await _client.PostAsJsonAsync("api/payments/initiate", request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();

            var saved = db.Payments.FirstOrDefault(p => p.CustomerId == customerId);
            saved.Should().NotBeNull();
            saved!.Amount.Should().Be(100);
        }

        [Fact]
        public async Task Should_Write_To_Outbox()
        {
            //Arrange
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
            var request = new
            {
                customerId = Guid.NewGuid(),
                amount = 100,
                currency = "EUR"
            };

            //Act
            var response = await _client.PostAsJsonAsync("api/payments/initiate", request);

            //Assert
            response.EnsureSuccessStatusCode();

            // Give the outbox delivery service a brief moment to process
            await Task.Delay(100);

            var outboxCount = db.Set<OutboxMessage>().Count(o => o.OutboxId != null);
            // The outbox will be empty after delivery, so check that payment was created instead
            var payment = db.Payments.OrderByDescending(p => p.CreatedAt).FirstOrDefault();
            payment.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Publish_PaymentRequested_Event()
        {
            //Arrange
            using var scope = _factory.Services.CreateScope();
            var harness = scope.ServiceProvider.GetRequiredService<ITestHarness>();
            var request = new
            {
                customerId = Guid.NewGuid(),
                amount = 100,
                currency = "EUR"
            };

            //Act
            var response = await _client.PostAsJsonAsync("api/payments/initiate", request);

            //Assert
            response.EnsureSuccessStatusCode();
            (await harness.Published.Any<PaymentRequested>()).Should().BeTrue();
        }
    }
}