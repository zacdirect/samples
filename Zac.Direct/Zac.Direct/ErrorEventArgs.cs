using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Zac.Direct
{
    [DataContract]
    public class ErrorEventArgs : TraceEventArgs, IBaseError
    {
        public ErrorEventArgs(string message) : this(message, message) { }

        public ErrorEventArgs(string message, string friendlyMessage) : this(message, friendlyMessage, "") { }

        public ErrorEventArgs(string message, string friendlyMessage, string source) : this(message, friendlyMessage, "", "", source) { }

        public ErrorEventArgs(string message, string friendlyMessage, string errorCode, string stackTrace, string source)
            : base(message, friendlyMessage, source)
        {
            Severity = ArgStatusType.Error;
            ErrorCode = errorCode;
            StackTrace = stackTrace;
        }

        public ErrorEventArgs(Exception e) : this(e, e.Message) { }

        public ErrorEventArgs(Exception e, string friendlyMessage)
            : this(e.Message, friendlyMessage, "", e.StackTrace, e.Source)
        {
            UnderlyingException = e;
        }

        public ErrorEventArgs(IBaseError e)
            : this(e.Message, e.FriendlyMessage, e.ErrorCode, e.StackTrace, e.Source)
        {
            Severity = ArgStatusType.Error;
        }

        [DataMember]
        public string ErrorCode { get; set; }
        [DataMember]
        public string StackTrace { get; set; }

        [IgnoreDataMember]
        public Exception UnderlyingException { get; set; }
    }
}
