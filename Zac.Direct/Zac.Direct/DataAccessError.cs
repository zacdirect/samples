using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Zac.Direct
{
    [DataContract]
    public class DataAccessError : ErrorEventArgs, IDataAccessError
    {
        public DataAccessError(string message) : this(message, message) { }

        public DataAccessError(string message, string friendlyMessage) : this(message, friendlyMessage, "") { }

        public DataAccessError(string message, string friendlyMessage, string source) : this(message, friendlyMessage, "", "", source) { }

        public DataAccessError(string message, string friendlyMessage, string errorCode, string stackTrace, string source)
            : base(message, friendlyMessage, source)
        {
            Severity = ArgStatusType.Error;
            ErrorCode = errorCode;
            StackTrace = stackTrace;
        }

        public DataAccessError(Exception e) : this(e, e.Message) { }

        public DataAccessError(Exception e, string friendlyMessage)
            : this(e.Message, friendlyMessage, "", e.StackTrace, e.Source)
        {
            UnderlyingException = e.InnerException ?? e;
        }

        public DataAccessError(IBaseError e)
            : this(e.Message, e.FriendlyMessage, e.ErrorCode, e.StackTrace, e.Source)
        {
            Severity = ArgStatusType.Error;
        }

    }
}
