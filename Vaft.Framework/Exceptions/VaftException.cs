using System;

namespace Vaft.Framework.Exceptions
{
    [Serializable]
    public class VaftException : Exception
    {
        public VaftException() { }

        public VaftException(string message) : base(message) { }

        public VaftException(string message, Exception inner) : base(message, inner) { }

        protected VaftException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
