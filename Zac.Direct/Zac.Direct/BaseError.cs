using System;
using System.Runtime.Serialization;

namespace Zac.Direct
{
    [DataContract]
    public class BaseError : BaseStatus, IBaseError
    {
        public BaseError()
            : base()
        {
            ErrorCode = string.Empty;
            StackTrace = string.Empty;
        }

        public BaseError(Exception e)
        {
            StackTrace = e.StackTrace;
            Message = e.Message;
            Severity = ArgStatusType.Error;
            Source = e.Source;
            FriendlyMessage = "An error has occurred while processing.";
        }

        [DataMember]
        public string ErrorCode { get; set; }

        [DataMember]
        public string StackTrace { get; set; }

    }
}
