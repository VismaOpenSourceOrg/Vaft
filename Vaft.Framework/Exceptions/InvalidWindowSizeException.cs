using System;

namespace Vaft.Framework.Exceptions
{
    public class InvalidWindowSizeException : VaftException
    {
        public InvalidWindowSizeException(string message, Exception ex) : base(message, ex) { }
    }
}
