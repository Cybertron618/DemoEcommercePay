using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace DemoEcommercePay.Api.src.Infrastructure.Kafka.Producers
{
    public class OrderProducer : IDisposable
    {
        private readonly ProducerConfig _producerConfig;
        private readonly string _topicName;
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<OrderProducer> _logger;

        public OrderProducer(string bootstrapServers, string topicName, ILogger<OrderProducer> logger)
        {
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            _topicName = topicName;
            _producer = new ProducerBuilder<string, string>(_producerConfig).Build();
            _logger = logger;
        }

        public async Task ProduceOrderAsync(string orderData)
        {
            try
            {
                var message = new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(), // Optional: set a message key for partitioning
                    Value = orderData
                };

                var deliveryReport = await _producer.ProduceAsync(_topicName, message);

                _logger.LogInformation("Produced message to topic '{Topic}' at partition {Partition} with offset {Offset}",
                    deliveryReport.Topic, deliveryReport.Partition, deliveryReport.Offset);
            }
            catch (ProduceException<string, string> e)
            {
                _logger.LogError("Error producing message: {ErrorMessage}", e.Error.Reason);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _producer?.Dispose();
            }
        }

        ~OrderProducer()
        {
            Dispose(false);
        }
    }
}
