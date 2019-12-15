using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization;

namespace Zac.Direct
{
    [DataContract]
    public class TraceEventArgs : EventArgs, IBaseStatus
    {
        public TraceEventArgs(string message) : this(message, "") { }

        public TraceEventArgs(string message, string friendlyMessage) : this(message, friendlyMessage, MethodInfo.GetCurrentMethod().Name) { }

        public TraceEventArgs(string message, string friendlyMessage, string source)
            : this(message, friendlyMessage, source, "") { }

        public TraceEventArgs(string message, string friendlyMessage, string source, string applicationStatusCode)
            : this(message, friendlyMessage, source, applicationStatusCode, null) { }

        public TraceEventArgs(string message, string friendlyMessage, string source, string applicationStatusCode, string[] applicationMessages)
        {

            Message = message;
            FriendlyMessage = friendlyMessage;
            Source = source;
            Severity = ArgStatusType.Trace;
            ApplicationStatusCode = applicationStatusCode;
            ApplicationMessages = applicationMessages ?? new string[] { };
        }


        public TraceEventArgs(IBaseStatus status)
            : this(status.Message, status.FriendlyMessage, status.Source)
        {
            Severity = status.Severity;
        }

        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string FriendlyMessage { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public ArgStatusType Severity { get; set; }
        [DataMember]
        public string ApplicationStatusCode { get; set; }
        [DataMember]
        public string[] ApplicationMessages { get; set; }

        public override string ToString()
        {
            var message = string.Format("{0}{1}{2}{3}",
                Message,
                string.IsNullOrEmpty(FriendlyMessage) ? "" : " (",
                string.IsNullOrEmpty(FriendlyMessage) ? "" : FriendlyMessage,
                string.IsNullOrEmpty(FriendlyMessage) ? "" : ")"
                );
            return string.Format("{0} : {1}", Source, message);
        }
    }
}
