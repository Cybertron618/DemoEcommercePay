using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DemoEcommercePay.Api.src.Domain.Entities;
using StackExchange.Redis;
using DemoEcommercePay.Api.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DemoEcommercePay.Api.src.Infrastructure.Repositories;
using DemoEcommercePay.Api.src.Infrastructure.Services;
using Infrastructure.Services;
using Confluent.Kafka;
using DemoEcommercePay.Api.src.Infrastructure.Kafka.Consumers;
using DemoEcommercePay.Api.src.Infrastructure.Kafka.Producers;
using Nest;

namespace DemoEcommercePay.Api.src.APIGateway
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //kafka
            var kafkaConfig = new ConsumerConfig
            {
                BootstrapServers = Configuration.GetConnectionString("KafkaBootstrapServers"),
                GroupId = Configuration.GetConnectionString("KafkaConsumerGroupId"),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            services.AddSingleton<ConsumerConfig>(kafkaConfig);
            services.AddScoped<OrderConsumer>();

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = Configuration.GetConnectionString("KafkaBootstrapServers")
            };
            services.AddSingleton<ProducerConfig>(producerConfig);
            services.AddScoped<OrderProducer>();
            //kafka

            // Add Redis distributed cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("RedisConnectionString");
                options.InstanceName = "DemoEcommercePay:";
            });
            //redis

            // Add DbContext
            services.AddDbContext<EcommerceContext>(options =>
        options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Add repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            // Add services
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
