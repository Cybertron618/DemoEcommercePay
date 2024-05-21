namespace DemoEcommercePay.Api.src.Shared
{
    public enum DocumentStatus
    {
        Draft,
        Published,
        Archived
    }

    public enum UserRole
    {
        Admin,
        User,
        Guest
    }

    public enum CacheExpiration
    {
        ShortTerm = 5, // 5 minutes
        MediumTerm = 30, // 30 minutes
        LongTerm = 60 // 60 minutes
    }

    public enum LogLevel
    {
        Trace,
        Debug,
        Information,
        Warning,
        Error,
        Critical
    }

    public enum KafkaEventType
    {
        OrderCreated,
        OrderUpdated,
        OrderDeleted
    }
}

