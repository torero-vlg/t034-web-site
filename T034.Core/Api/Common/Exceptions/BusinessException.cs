using System;

namespace T034.Core.Api.Common.Exceptions
{
    /// <summary>
    /// Исключение, произошедшее в бизнес логике
    /// </summary>
    public class BusinessException : ApplicationException
    {
        public BusinessException()
        {

        }

        public BusinessException(string message) : base(message)
        {

        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
