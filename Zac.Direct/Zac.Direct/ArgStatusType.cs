using System;
using System.Runtime.Serialization;

namespace Zac.Direct
{
    [Flags]
    [DataContract]
    public enum ArgStatusType
    {
        /// <summary>
        /// No status
        /// </summary>
        [EnumMember]
        None = 1,
        /// <summary>
        /// General Trace
        /// </summary>
        [EnumMember]
        Trace = 2,  
        /// <summary>
        /// General Info for client application level
        /// </summary>
        [EnumMember]
        Info = 4,     
        /// <summary>
        /// General Warning Info for client application level
        /// </summary>
        [EnumMember]
        Warning = 8,         
        /// <summary>
        /// General Error Info for client application level
        /// </summary>
        [EnumMember]
        Error = 16 

    }
}
