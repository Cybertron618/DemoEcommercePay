using System;

namespace DemoEcommercePay.Api.src.Domain.Utilities
{
    public static class Helper
    {
        public static string GenerateOrderNumber()
        {
            return Guid.NewGuid().ToString().Replace("-", "")[..10].ToUpper();
        }
    }
}
