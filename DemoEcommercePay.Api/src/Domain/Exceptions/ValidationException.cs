using System;

namespace DemoEcommercePay.Api.src.Domain.Exceptions
{
    public class ValidationException(string message) : Exception(message)
    {
    }
}
