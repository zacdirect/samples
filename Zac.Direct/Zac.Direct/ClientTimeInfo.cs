using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zac.Direct
{
    public class ClientTimeInfo
    {
        public TimeZoneInfo ClientTimeZone { get; set; }
        public string ClientTimeZoneName { get; set; }
        public string ClientTimeZoneOffset { get; set; }
        public bool IsClientZoneDst { get; set; }
        public string ClientCurrentTimeStamp { get; set; }
        public DateTime? ClientCurrentDateTime { get; set; }
    }
}
