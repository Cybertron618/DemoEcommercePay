using System;

namespace DemoEcommercePay.Api.src.Domain.Utilities
{
    public static class Extensions
    {
        public static void Print(this object obj)
        {
            Console.WriteLine(obj.ToString());
        }
    }
}
