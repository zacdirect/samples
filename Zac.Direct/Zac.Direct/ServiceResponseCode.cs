using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Zac.Direct
{
    [DataContract]
    public enum ServiceResponseCode
    {
        [EnumMember]
        NotProcessed = 0,
        [EnumMember]
        Processing = 1,
        [EnumMember]
        Success = 2,
        [EnumMember]
        Error = 3
    }
}
