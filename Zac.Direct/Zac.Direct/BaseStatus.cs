using System;
using System.Runtime.Serialization;

namespace Zac.Direct
{
    [DataContract]
    public class BaseStatus : IBaseStatus
    {
        public BaseStatus()
        {
            Message = string.Empty;
            FriendlyMessage = string.Empty;
            Source = string.Empty;
            Severity = ArgStatusType.None;
        }

        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string FriendlyMessage { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public ArgStatusType Severity { get; set; }
    }
}
