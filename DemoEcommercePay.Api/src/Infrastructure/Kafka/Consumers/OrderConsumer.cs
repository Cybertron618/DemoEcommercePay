using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace DemoEcommercePay.Api.src.Infrastructure.Kafka.Consumers
{
    public class OrderConsumer : IDisposable
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly string _topicName;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<OrderConsumer> _logger;
        private bool _isConsuming;

        public OrderConsumer(string bootstrapServers, string groupId, string topicName, ILogger<OrderConsumer> logger)
        {
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false, 
                EnablePartitionEof = true 
            };

            _topicName = topicName;
            _cancellationTokenSource = new CancellationTokenSource();
            _consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();
            _logger = logger;
            _isConsuming = false;
        }

        public async Task StartConsumingAsync()
        {
            if (!_isConsuming)
            {
                _consumer.Subscribe(_topicName);
                _isConsuming = true;

                try
                {
                    while (!_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        try
                        {
                            var consumeResult = await Task.Run(() => _consumer.Consume(_cancellationTokenSource.Token));

                            if (consumeResult.IsPartitionEOF)
                            {
                                _logger.LogInformation("Reached end of partition: {PartitionOffset}", consumeResult.TopicPartitionOffset);
                                continue;
                            }

                            // Process the received order event here
                            _logger.LogInformation("Received message: {MessageValue}", consumeResult.Message.Value);

                            _consumer.Commit(consumeResult); // Manually commit offset if auto-commit is disabled
                        }
                        catch (OperationCanceledException)
                        {
                            // Handle cancellation
                        }
                        catch (ConsumeException e)
                        {
                            _logger.LogError("Error consuming message: {ErrorMessage}", e.Error.Reason);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in consumer loop: {ErrorMessage}", ex.Message);
                }

            }
            else
            {
                _logger.LogWarning("Consumer is already consuming messages.");
            }
        }

        public void StopConsuming()
        {
            if (_isConsuming)
            {
                _consumer.Close();
                _cancellationTokenSource.Cancel();
                _isConsuming = false;
            }
            else
            {
                _logger.LogWarning("Consumer is not currently consuming messages.");
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
                _consumer?.Dispose();
                _cancellationTokenSource?.Dispose();
            }
        }

        ~OrderConsumer()
        {
            Dispose(false);
        }
    }
}
