using System;

namespace Infrastructure.Application.Core
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) { }
    }
}