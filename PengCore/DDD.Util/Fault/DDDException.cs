using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DDD.Util.Fault
{
    /// <summary>
    /// micbeach framework exception
    /// </summary>
    public class DDDException : Exception
    {
        public DDDException(){ }

        public DDDException(string message) : base(message) { }

        public DDDException(string message, Exception innerException) : base(message, innerException) { }

        protected DDDException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
