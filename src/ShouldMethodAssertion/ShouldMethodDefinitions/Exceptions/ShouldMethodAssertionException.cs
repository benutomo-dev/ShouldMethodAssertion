
using System.Runtime.Serialization;

namespace Xunit.Sdk
{
    internal sealed class ShouldMethodAssertionException : Exception
    {
        public ShouldMethodAssertionException()
        {
        }

        public ShouldMethodAssertionException(string? message) : base(message)
        {
        }

        public ShouldMethodAssertionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

#pragma warning disable SYSLIB0051 // 型またはメンバーが旧型式です
        public ShouldMethodAssertionException(SerializationInfo info, StreamingContext context) : base(info, context)
#pragma warning restore SYSLIB0051 // 型またはメンバーが旧型式です
        {
        }
    }
}

namespace ShouldMethodAssertion.ShouldMethodDefinitions.Exceptions
{
    internal sealed class ShouldMethodAssertionException : Exception
    {
        public ShouldMethodAssertionException()
        {
        }

        public ShouldMethodAssertionException(string? message) : base(message)
        {
        }

        public ShouldMethodAssertionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }


#pragma warning disable SYSLIB0051 // 型またはメンバーが旧型式です
        public ShouldMethodAssertionException(SerializationInfo info, StreamingContext context) : base(info, context)
#pragma warning restore SYSLIB0051 // 型またはメンバーが旧型式です
        {
        }
    }
}
