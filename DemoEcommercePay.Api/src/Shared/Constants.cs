namespace DemoEcommercePay.Api.src.Shared
{
    public static class Constants
    {
        //constants
        public const string DateFormat = "yyyy-MM-dd";
        public const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
        public const string DefaultCacheKeyPrefix = "DemoEcommercePay";

        // Error messages
        public const string DocumentNotFound = "Document not found";
        public const string UserNotFound = "User not found";
        public const string UnauthorizedAccess = "Unauthorized access";

        // Configuration keys
        public const string ElasticsearchUri = "Elasticsearch:Uri";
        public const string RedisConnectionString = "Redis:ConnectionString";

        // Kafka topics
        public const string OrderTopic = "orders";
    }
}

