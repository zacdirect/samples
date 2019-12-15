using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Zac.Direct
{
    [DataContract]
    public class InfoEventArgs : TraceEventArgs
    {
        public InfoEventArgs(string message)
            : this(message, null, "")
        {

        }

        public InfoEventArgs(string message, string applicationStatus)
            : this(message, null, applicationStatus)
        {

        }

        public InfoEventArgs(string message, string[] applicationMessages, string applicationStatus)
            : base(message)
        {
            Severity = ArgStatusType.Info;
            ApplicationStatusCode = applicationStatus;
            ApplicationMessages = applicationMessages ?? new string[] { };
        }
    }
}
